// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using AForge.Math.Geometry;

namespace AForge.Vision.GlyphRecognition
{
    public class GlyphTracker
    {
        private class TrackedGlyph
        {
            public readonly int ID;
            public readonly ExtractedGlyphData Glyph;

            public int Age = 0;
            public Point Position;

            public TrackedGlyph( int id, ExtractedGlyphData glyph, Point position  )
            {
                ID = id;
                Glyph = glyph;
                Position = position;
            }
        }

        private const int MaxGlyphAge = 10;
        private const int MaxGlyphShaking = 1;

        private int counter = 1;

        private Dictionary<int, TrackedGlyph> trackedGlyphs = new Dictionary<int, TrackedGlyph>( );
        
        // Get list of IDs for the corresponding glyphs
        public List<int> IdentifyGlyphs( List<ExtractedGlyphData> glyphs )
        {
            // process previously tracked glyphs
            IncreaseHistoryAge( );

            List<int> glyphIDs = new List<int>( );

            // get ID of each found glyph
            foreach ( ExtractedGlyphData glyph in glyphs )
            {
                glyphIDs.Add( GetGlyphID( glyph ) );
            }

            return glyphIDs;
        }

        // Get ID of the specified glyph
        // Note (todo?): glyph tracking need to be improved since current implementation
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
                if ( ( glyph.RecognizedGlyph != null ) &&
                     ( GetMaxCoordinateDiff( glyph.RecognizedQuadrilateral,
                       trackedGlyphs[glyphID].Glyph.RecognizedQuadrilateral ) <= MaxGlyphShaking ) )
                {
                    // correct coordinates of recognized glyphs to eliminate small noisy shaking
                    glyph.RecognizedQuadrilateral = trackedGlyphs[glyphID].Glyph.RecognizedQuadrilateral;
                    // reset age of the tracked glyph
                    trackedGlyphs[glyphID].Age = 0;
                }
                else
                {
                    // update glyph with the latest CoG and recognized info
                    trackedGlyphs[glyphID] = new TrackedGlyph( glyphID,
                        (ExtractedGlyphData) glyph.Clone( ), glyphCog );
                }
            }

            return glyphID;
        }

        // Increase age of tracked glyph and remove old ones
        private void IncreaseHistoryAge( )
        {
            List<int> keys = new List<int>( trackedGlyphs.Keys );

            for ( int i = 0; i < keys.Count; i++ )
            {
                int id = keys[i];

                if ( ++trackedGlyphs[id].Age > MaxGlyphAge )
                {
                    trackedGlyphs.Remove( id );
                }
            }
        }

        // Reset glyph tracker to initial state
        public void Reset( )
        {
            trackedGlyphs.Clear( );
        }

        // Calculate maximum difference between coordinates of the specfied points
        private static int GetMaxCoordinateDiff( List<IntPoint> points1, List<IntPoint> points2 )
        {
            int maxDiff = 0;

            for ( int i = 0, n = points1.Count; i < n; i++ )
            {
                maxDiff = System.Math.Max( maxDiff, System.Math.Abs( points1[i].X - points2[i].X ) );
                maxDiff = System.Math.Max( maxDiff, System.Math.Abs( points1[i].Y - points2[i].Y ) );
            }

            return maxDiff;
        }
    }
}
