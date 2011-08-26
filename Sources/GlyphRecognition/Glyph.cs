// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;

    /// <summary>
    /// Square binary glyph.
    /// </summary>
    public class Glyph : ICloneable
    {
        private string name;
        private byte[,] data;
        private object userData;

        /// <summary>
        /// Glyph's name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Glyph's size - length of <see cref="Data"/> array dimenstions.
        /// </summary>
        public int Size
        {
            get { return data.GetLength( 0 ); }
        }

        /// <summary>
        /// Glyph's data array.
        /// </summary>
        /// 
        /// <remarks><para>The array contains 0 and 1 elements, where 0 corresponds to black areas of the glyph
        /// and 1 corresponds to white areas.</para>
        /// 
        /// <para><note>The data array must be square.</note></para>
        /// </remarks>
        /// 
        /// <exception cref="NullReferenceException">The data array cannot be null.</exception>
        /// <exception cref="ArgumentException">Both dimensions of the array must have the same length.</exception>
        /// 
        public byte[,] Data
        {
            get { return data; }
            set
            {
                if ( value == null )
                {
                    throw new NullReferenceException( "The data array cannot be null." );
                }

                if ( value.GetLength( 0 ) != value.GetLength( 1 ) )
                {
                    throw new ArgumentException( "Both dimensions of the array must have the same length." );
                }

                data = value;
            }
        }

        /// <summary>
        /// User's data associated with the glyph.
        /// </summary>
        /// 
        /// <remarks><para>The property allows to associate any user data with the glyph, like glyph's visualization
        /// parameters for example.</para></remarks>
        /// 
        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Glyph"/> class.
        /// </summary>
        /// 
        /// <param name="name">Glyph's name.</param>
        /// <param name="size">Glyph's size.</param>
        /// 
        /// <remarks><para>Creates an empty <paramref name="size"/>x<paramref name="size"/> glyph
        /// (all values are set to 0 - black).</para></remarks>
        /// 
        public Glyph( string name, int size )
        {
            this.name = name;
            this.data = new byte[size, size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Glyph"/> class.
        /// </summary>
        /// 
        /// <param name="name">Glyph's name.</param>
        /// <param name="data">Glyph's data array (see <see cref="Data"/>).</param>
        /// 
        public Glyph( string name, byte[,] data )
        {
            Name = name;
            Data = data;
        }

        /// <summary>
        /// Clone the glyph.
        /// </summary>
        /// 
        /// <returns>Returns clone of the glyph.</returns>
        /// 
        /// <remarks><para><note>It is user's responsibility to clone <see cref="UserData"/> property if it is
        /// set to reference type object.</note></para></remarks>
        /// 
        public object Clone( )
        {
            Glyph clone = new Glyph( name, (byte[,]) data.Clone( ) );

            clone.userData = userData;

            return clone;
        }

        /// <summary>
        /// Check for matching between the glyph and specified raw glyph data.
        /// </summary>
        /// 
        /// <param name="rawGlyphData">Glyph data to check match with.</param>
        /// 
        /// <returns>Returns -1 if there is no matching between the glyph and the specified glyph data.
        /// In the case if match is found it returns:
        /// <list type="bullets">
        /// <item>0 - the glyph matches with specified glyph data as they are provided;</item>
        /// <item>90, 180 or 270 - the glyph matches with specified data if they are rotated by 90, 180 or 270
        /// degree respectively in counter clockwise direction.</item>
        /// </list></returns>
        /// 
        /// <exception cref="ArgumentException">Invalid glyph data array - must be square.</exception>
        /// 
        public int CheckForMatching( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != rawGlyphData.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square." );
            }

            if ( size != data.GetLength( 0 ) )
                return -1;

            int sizeM1 = size - 1;

            bool match1 = true;
            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    // no rotation
                    match1 &= ( value == data[i, j] );
                    // 180 deg
                    match2 &= ( value == data[sizeM1 - i, sizeM1 - j] );
                    // 90 deg
                    match3 &= ( value == data[sizeM1 - j, i] );
                    // 270 deg
                    match4 &= ( value == data[j, sizeM1 - i] );
                }
            }

            if ( match1 )
                return 0;
            else if ( match2 )
                return 180;
            else if ( match3 )
                return 90;
            else if ( match4 )
                return 270;

            return -1;
        }

        /// <summary>
        /// Check for matching between raw data of two glyphs.
        /// </summary>
        /// 
        /// <param name="sourceGlyph">Source glyph data to check.</param>
        /// <param name="targetGlyph">Target glyph data to match with.</param>
        /// 
        /// <returns>Returns -1 if there is no matching between two specified glyphs.
        /// In the case if match is found it returns:
        /// <list type="bullets">
        /// <item>0 - the two glyphs match each other as they are provided;</item>
        /// <item>90, 180 or 270 - the source glyph matches with target glyph if it is rotated by 90, 180 or 270
        /// degree respectively in counter clockwise direction.</item>
        /// </list></returns>
        /// 
        /// <exception cref="ArgumentException">Invalid glyph data array - must be square.</exception>
        /// 
        public static int CheckForMatching( byte[,] sourceGlyph, byte[,] targetGlyph )
        {
            int size = sourceGlyph.GetLength( 0 );

            if ( size != sourceGlyph.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square.", "rawGlyphData" );
            }

            if ( targetGlyph.GetLength( 0 ) != targetGlyph.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square.", "targetGlyph" );
            }

            if ( size != targetGlyph.GetLength( 0 ) )
                return -1;

            int sizeM1 = size - 1;

            bool match1 = true;
            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    byte value = sourceGlyph[i, j];

                    // no rotation
                    match1 &= ( value == targetGlyph[i, j] );
                    // 180 deg
                    match2 &= ( value == targetGlyph[sizeM1 - i, sizeM1 - j] );
                    // 90 deg
                    match3 &= ( value == targetGlyph[sizeM1 - j, i] );
                    // 270 deg
                    match4 &= ( value == targetGlyph[j, sizeM1 - i] );
                }
            }

            if ( match1 )
                return 0;
            else if ( match2 )
                return 180;
            else if ( match3 )
                return 90;
            else if ( match4 )
                return 270;

            return -1;
        }

        /// <summary>
        /// Check if the specified glyph data are rotation invariant or not.
        /// </summary>
        /// 
        /// <param name="rawGlyphData">Glyph data to check for rotation invariance.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if the specified glyph data are rotation
        /// invariant or <see langword="false"/> otherwise.</returns>
        /// 
        /// <remarks><para>Rotation invariant glyph means that it looks the same in case if rotated by 90 or 180
        /// degrees. In most applications (like augmented reality) glyphs must be rotation variant, so it could
        /// be possible to recognize rotation of the glyph. But for some applications, where glyph are not supposed
        /// to be rotated, the rotation invariance is also acceptable.</para></remarks>
        /// 
        /// <exception cref="ArgumentException">Invalid glyph data array - must be square.</exception>
        /// 
        public static bool CheckIfRotationInvariant( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != rawGlyphData.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square." );
            }

            int sizeM1 = size - 1;

            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    // 180 deg
                    match2 &= ( value == rawGlyphData[sizeM1 - i, sizeM1 - j] );
                    // 90 deg
                    match3 &= ( value == rawGlyphData[sizeM1 - j, i] );
                    // 270 deg
                    match4 &= ( value == rawGlyphData[j, sizeM1 - i] );
                }
            }

            return ( match2 || match3 || match4 );
        }

        /// <summary>
        /// Check if the specified raw glyph's data has a value in each inner row/column.
        /// </summary>
        /// 
        /// <param name="rawGlyphData">Glyph data to check.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if the specified glyph data contain
        /// a value (elemenent equal to 1) in every inner row/column
        /// or <see langword="false"/> otherwise.</returns>
        /// 
        /// <remarks><para>The tool method is used to check if specified glyph data have at least
        /// single element equal to 1 in every row/column except the first and the last.
        /// </para></remarks>
        /// 
        /// <exception cref="ArgumentException">Invalid glyph data array - must be square.</exception>
        /// 
        public static bool CheckIfEveryRowColumnHasValue( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != rawGlyphData.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square." );
            }

            int sizeM1 = size - 1;

            byte[] rows = new byte[sizeM1];
            byte[] cols = new byte[sizeM1];

            for ( int i = 1; i < sizeM1; i++ )
            {
                for ( int j = 1; j < sizeM1; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    rows[i] |= value;
                    cols[j] |= value;
                }
            }

            for ( int i = 1; i < sizeM1; i++ )
            {
                if ( ( rows[i] == 0 ) || ( cols[i] == 0 ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if the specified raw glyph's data have a border of 0 elements.
        /// </summary>
        /// 
        /// <param name="rawGlyphData">Glyph data to check.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if the specified glyph data contain
        /// 0 elements only in the first and the last row and column
        /// or <see langword="false"/> otherwise.</returns>
        /// 
        /// <exception cref="ArgumentException">Invalid glyph data array - must be square.</exception>
        /// 
        public static bool CheckIfGlyphHasBorder( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != rawGlyphData.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array - must be square." );
            }

            int sizeM1 = size - 1;

            for ( int i = 0; i <= sizeM1; i++ )
            {
                if ( rawGlyphData[0, i] == 1 )
                    return false;
                if ( rawGlyphData[sizeM1, i] == 1 )
                    return false;

                if ( rawGlyphData[i, 0] == 1 )
                    return false;
                if ( rawGlyphData[i, sizeM1] == 1 )
                    return false;
            }

            return true;
        }
    }
}
