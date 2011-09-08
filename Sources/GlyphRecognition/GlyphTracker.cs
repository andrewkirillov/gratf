// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Collections.Generic;

    using AForge.Math;
    using AForge.Math.Geometry;
    using AForge.Imaging;

    /// <summary>
    /// Glyph tracker.
    /// </summary>
    /// 
    /// <remarks><para>The purpose of this class is to perform tracking of glyphs, which are
    /// typically recognized by <see cref="GlyphRecognizer.FindGlyphs(UnmanagedImage)"/> routine or
    /// similar. The main purpose of this class is to provide 3D pose estimation of glyphs as well
    /// as provide glyphs' IDs, which could be used by user to perform glyphs' tracking in continuous
    /// video feed. See <see cref="TrackGlyphs"/> method for additional information.</para>
    /// </remarks>
    /// 
    public class GlyphTracker
    {
        private int counter = 1;
        private Dictionary<int, TrackedGlyph> trackedGlyphs = new Dictionary<int, TrackedGlyph>( );
        private Dictionary<int, Matrix3x3> prevRotation = new Dictionary<int, Matrix3x3>( );

        // TODO: age constants should be changed to time, not number of frames
        private const int MaxGlyphAge = 30;
        private const int MaxStalledGlyphAge = 1000;
        private const int MaxGlyphShaking = 1;
        private const int MaxAllowedDistance = 150;

        private const int StalledAverageMotionLimit = 2;

        // size of the image containing glyphs
        private System.Drawing.Size imageSize = new System.Drawing.Size( 640, 480 );
        // camera's effective focal length
        private float cameraFocalLength = 640;
        // real size of glyphs
        private float glyphSize = 0;

        private CoplanarPosit posit;

        /// <summary>
        /// Size of the image containing glyphs.
        /// </summary>
        /// 
        /// <remarks><para>The property sets size of the image from which tracked glyphs are extracted.</para>
        /// 
        /// <para>Default value is set to <b>(640, 480)</b>.</para>
        /// </remarks>
        /// 
        public System.Drawing.Size ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }

        /// <summary>
        /// Effective focal length of camera.
        /// </summary>
        /// 
        /// <remarks><para>The property sets effective focal length of the camera used
        /// to capture glyphs being tracked.</para>
        /// 
        /// <para><note>In many cases the value can be set to width of the image containing glyphs
        /// to achieve reasonable results. However the property is provided to let user make
        /// decision about it.</note></para>
        ///
        /// <para>Default value is set to <b>640</b>.</para>
        /// </remarks>
        /// 
        public float CameraFocalLength
        {
            get { return cameraFocalLength; }
            set
            {
                cameraFocalLength = value;

                if ( posit != null )
                {
                    posit.FocalLength = cameraFocalLength;
                }
            }
        }

        /// <summary>
        /// Real size of tracked glyphs.
        /// </summary>
        /// 
        /// <remarks><para>The property sets real size of glyphs being tracked. The class
        /// makes assumption that glyphs are square objects, so specifying size of one
        /// glyph's side is enough.</para>
        /// 
        /// <para>It is up to user to choose units for this property. Just keep in mind that
        /// glyph's translation which is part of glyph's
        /// <see cref="ExtractedGlyphData.TransformationMatrix">transformation matrix</see>
        /// will have same units.</para>
        /// 
        /// <para>If the property is set to <b>0</b>, then glyphs' pose estimation will not
        /// be performed.</para>
        /// 
        /// <para>Default value is set to <b>0</b>. The property can not be negative.</para>
        /// </remarks>
        /// 
        public float GlyphSize
        {
            get { return glyphSize; }
            set
            {
                glyphSize = Math.Max( 0, value );
                CreatePosit( );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphTracker"/> class.
        /// </summary>
        /// 
        public GlyphTracker( )
        {
            CreatePosit( );        
        }

        private void CreatePosit( )
        {
            if ( glyphSize != 0 )
            {
                // create glyph's model
                float sizeHalf = glyphSize / 2;

                Vector3[] glyphModel = new Vector3[]
                {
                    new Vector3( -sizeHalf, 0,  sizeHalf ),
                    new Vector3(  sizeHalf, 0,  sizeHalf ),
                    new Vector3(  sizeHalf, 0, -sizeHalf ),
                    new Vector3( -sizeHalf, 0, -sizeHalf ),
                };

                posit = new CoplanarPosit( glyphModel, cameraFocalLength );
            }
            else
            {
                posit = null;
            }
        }

        /// <summary>
        /// Track glyphs in continuous video feed.
        /// </summary>
        /// 
        /// <param name="glyphs">List of glyphs to track.</param>
        /// 
        /// <returns>Return IDs of the specified glyphs</returns>
        /// 
        /// <remarks><para>The method perform tracking of glyphs in continuous video feed. The
        /// specified list of <paramref name="glyphs"/> is supposed to be collected using <see cref="GlyphRecognizer.FindGlyphs(UnmanagedImage)"/>
        /// or similar method.</para>
        /// 
        /// <para>The main purpose of the method is to perform glyphs’ 3D pose estimation and reduce
        /// glyphs’ vibration (if glyph's corners do small negligible movement between continuous frames
        /// (kind of shacking), then it is treated as noise and glyphs pose is taken from the history).
        /// The estimated pose is provided as <see cref="ExtractedGlyphData.TransformationMatrix"/> and
        /// the <see cref="ExtractedGlyphData.IsTransformationDetected"/> property is set in this case as well.
        /// If movement noise suppression is performed, then <see cref="ExtractedGlyphData.RecognizedQuadrilateral"/>
        /// property is updated with coordinates from previous frame (which means noise suppression is performed
        /// for recognized glyphs only).</para>
        /// 
        /// <para>As an extra bonus the method provides list of glyphs' IDs - each detected glyph gets
        /// an ID so it could be tracked between continuous video frames. <b>Note:</b> these IDs should not
        /// be considered 100% reliable as ID of the same glyph may change if it disappears for a while
        /// or does not move smoothly.</para>
        /// </remarks>
        /// 
        public List<int> TrackGlyphs( List<ExtractedGlyphData> glyphs )
        {
            // process previously tracked glyphs
            IncreaseHistoryAge( );

            List<int> glyphIDs = new List<int>( );

            // get ID of each found glyph
            foreach ( ExtractedGlyphData glyph in glyphs )
            {
                int glyphID = GetGlyphID( glyph );

                glyphIDs.Add( glyphID );

                if ( ( glyphSize != 0 ) && ( glyph.RecognizedGlyph != null ) )
                {
                    EstimateGlyphPose( glyph, glyphID );
                }
            }

            return glyphIDs;
        }

        // Get ID of the specified glyph
        // Note (todo?): glyph tracking needs to be improved since current implementation
        // is not reliable enough for the case when several glyphs of the same type are
        // found and those do not move smoothely.
        private int GetGlyphID( ExtractedGlyphData glyph )
        {
            int glyphID = -1;

            // get CoG of the provided glyph
            Point glyphCog = PointsCloud.GetCenterOfGravity( glyph.Quadrilateral );
            // distance to the closest simlar glyph kept in history
            float minDistance = float.MaxValue;

            // name of the passed specifed glyph
            string glyphName = ( glyph.RecognizedGlyph != null ) ?
                glyph.RecognizedGlyph.Name : string.Empty;

            // check all currently tracked glyphs
            foreach ( TrackedGlyph trackedGlyph in trackedGlyphs.Values )
            {
                if ( trackedGlyph.Age == 0 )
                {
                    // skip glyph whichs were already taken or just added
                    continue;
                }

                if ( 
                     // recognized case - compare names
                     ( ( trackedGlyph.Glyph.RecognizedGlyph != null ) &&
                     ( trackedGlyph.Glyph.RecognizedGlyph.Name == glyphName ) ) ||
                     // unrecognized case - compare raw data
                     ( Glyph.CheckForMatching( glyph.RawData, trackedGlyph.Glyph.RawData ) != -1 ) )
                {
                    float distance = glyphCog.DistanceTo( trackedGlyph.Position );

                    if ( distance < minDistance )
                    {
                        // get ID of the closest glyph with the same name
                        minDistance = distance;
                        glyphID = trackedGlyph.ID;
                    }
                }
            }

            // if the glyph is further away than the maximum specified limit,
            // then it is no way can be treated as smooth motion, so reset ID
            // (TODO: should probably depend on glyph size ...)
            if ( ( glyphID != -1 ) && ( minDistance > MaxAllowedDistance ) )
            {
                glyphID = -1;
            }

            // if glyph was not found within tracked glyphs, then add new
            // glyph to tracker
            if ( glyphID == -1 )
            {
                glyphID = counter++;
                trackedGlyphs.Add( glyphID, new TrackedGlyph( glyphID,
                    (ExtractedGlyphData) glyph.Clone( ), glyphCog ) );
            }
            else
            {
                TrackedGlyph trackedGlyph = trackedGlyphs[glyphID];

                if ( ( glyph.RecognizedGlyph != null ) &&
                     ( !IsCoordinatesDifferenceSignificant( glyph.RecognizedQuadrilateral,
                       trackedGlyphs[glyphID].Glyph.RecognizedQuadrilateral ) ) )
                {
                    // correct coordinates of recognized glyphs to eliminate small noisy shaking
                    glyph.RecognizedQuadrilateral = trackedGlyph.Glyph.RecognizedQuadrilateral;
                    glyphCog = trackedGlyph.Position;
                }
                else
                {
                    // update glyph with the latest CoG and recognized info
                    trackedGlyph.Position = glyphCog;
                    trackedGlyph.Glyph = (ExtractedGlyphData) glyph.Clone( );
                }

                // reset age of the tracked glyph
                trackedGlyph.Age = 0;

                // add glyph's position to history
                trackedGlyph.AddMotionHistory( glyphCog );
            }

            return glyphID;
        }

        /// <summary>
        /// Reset internal state of the tracker.
        /// </summary>
        /// 
        /// <remarks><para>The method resets all internal variables of the tracker to the
        /// initial state so it is ready to be used on different video feed.</para></remarks>
        /// 
        public void Reset( )
        {
            trackedGlyphs.Clear( );
            prevRotation.Clear( );
        }

        // Check if difference between glyphs corners' coordinates is significant (more
        // than caused by noise)
        private static bool IsCoordinatesDifferenceSignificant( List<IntPoint> points1, List<IntPoint> points2 )
        {
            int significantDifferences = 0;

            for ( int i = 0, n = points1.Count; i < n; i++ )
            {
                if ( ( System.Math.Abs( points1[i].X - points2[i].X ) > MaxGlyphShaking ) ||
                     ( System.Math.Abs( points1[i].Y - points2[i].Y ) > MaxGlyphShaking ) )
                {
                    significantDifferences++;
                }
            }

            // if glyph starts moving/rotating, then at least two corners should change position.
            // if only one changes, then it is caused by noise most probably
            return ( significantDifferences > 1 );
        }

        // Estimate pose for of the given glyph
        private void EstimateGlyphPose( ExtractedGlyphData glyph, int glyphID )
        {
            int imageCenterX = imageSize.Width >> 1;
            int imageCenterY = imageSize.Height >> 1;

            Point[] glyphPoints = new Point[4];
            Matrix3x3 rotation;
            Vector3 translation;

            // get array of points with coordinates in Cartesian system with origin in image center
            for ( int i = 0; i < 4; i++ )
            {
                glyphPoints[i] = new Point(
                    glyph.RecognizedQuadrilateral[i].X - imageCenterX,
                    imageCenterY - glyph.RecognizedQuadrilateral[i].Y );
            }

            // estimate pose using Coplanar POSIT algorithm
            posit.EstimatePose( glyphPoints, out rotation, out translation );

            glyph.TransformationMatrix = Matrix4x4.CreateTranslation( translation ) *
                                         Matrix4x4.CreateFromRotation( rotation );
            glyph.IsTransformationDetected = true;

            // check if we have previous rotation of the glyph
            if ( !prevRotation.ContainsKey( glyphID ) )
            {
                // remember it if not
                prevRotation.Add( glyphID, posit.BestEstimatedRotation );
            }
            else
            {
                Matrix3x3 newRotation = posit.BestEstimatedRotation;

                // check if best estimation is at least twice is better than the alternate
                // (better according to the POSIT algorithm)
                if ( posit.AlternateEstimationError / posit.BestEstimationError < 2 )
                { 
                    // error difference is not very big, so compare both transformations with previous
                    // and select the one which seems to be closer to it
                    Matrix3x3 d1 = prevRotation[glyphID] - posit.BestEstimatedRotation;
                    Matrix3x3 d2 = prevRotation[glyphID] - posit.AlternateEstimatedRotation;

                    float e1 = d1.GetRow( 0 ).Square + d1.GetRow( 1 ).Square + d1.GetRow( 2 ).Square;
                    float e2 = d2.GetRow( 0 ).Square + d2.GetRow( 1 ).Square + d2.GetRow( 2 ).Square;

                    if ( e1 > e2 )
                    {
                        glyph.TransformationMatrix =
                            Matrix4x4.CreateTranslation( posit.AlternateEstimatedTranslation ) *
                            Matrix4x4.CreateFromRotation( posit.AlternateEstimatedRotation );

                        newRotation = posit.AlternateEstimatedRotation;
                    }
                }

                prevRotation[glyphID] = newRotation;
            }
        }

        // Increase age of tracked glyph and remove old ones
        private void IncreaseHistoryAge( )
        {
            List<int> keys = new List<int>( trackedGlyphs.Keys );

            for ( int i = 0; i < keys.Count; i++ )
            {
                int id = keys[i];
                TrackedGlyph trackedGlyph = trackedGlyphs[id];

                trackedGlyph.Age++;

                if ( ( ( trackedGlyph.Age > MaxGlyphAge ) && ( trackedGlyph.AverageRecentMotion >= StalledAverageMotionLimit ) ) ||
                       ( trackedGlyph.Age > MaxStalledGlyphAge ) )
                {
                    trackedGlyphs.Remove( id );

                    if ( prevRotation.ContainsKey( id ) )
                    {
                        prevRotation.Remove( id );
                    }
                }
            }
        }

        // Information about the the tracked glyph
        private class TrackedGlyph
        {
            private const int MaxMotionHistoryLength = 11;
            private const int RecentStepsCount = 10;

            public readonly int ID;

            public ExtractedGlyphData Glyph;
            public int Age = 0;
            public Point Position;

            private readonly List<Point> motionHistory = new List<Point>( );
            public double RecentPathLength = 0;
            public double AverageRecentMotion = 0;

            public TrackedGlyph( int id, ExtractedGlyphData glyph, Point position )
            {
                ID = id;
                Glyph = glyph;
                Position = position;
            }

            public void AddMotionHistory( Point position )
            {
                motionHistory.Add( position );

                if ( motionHistory.Count > MaxMotionHistoryLength )
                {
                    motionHistory.RemoveAt( 0 );
                }

                // calculate amount of recent movement
                RecentPathLength = 0;
                int stepsCount = Math.Min( RecentStepsCount, motionHistory.Count - 1 );
                int historyLimit = MaxMotionHistoryLength - stepsCount;

                for ( int i = motionHistory.Count - 1; i >= historyLimit; i-- )
                {
                    RecentPathLength += motionHistory[i].DistanceTo( motionHistory[i - 1] );
                }

                AverageRecentMotion = ( stepsCount == 0 ) ? 0 : RecentPathLength / stepsCount;
            }
        }
    }
}
