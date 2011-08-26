// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Glyphs' database.
    /// </summary>
    /// 
    /// <remarks><para>The class represents collection of glyphs, which cab be recognized with the help of
    /// <see cref="GlyphRecognizer"/>.</para></remarks>
    /// 
    public class GlyphDatabase : IEnumerable<Glyph>
    {
        private Dictionary<string, Glyph> glyphs = new Dictionary<string,Glyph>( );

        // size of glyphs in the database
        private int size;

        /// <summary>
        /// Get glyph's enumerator.
        /// </summary>
        /// 
        /// <returns>Returns glyph's enumerator.</returns>
        /// 
        public IEnumerator<Glyph> GetEnumerator( )
        {
            foreach ( KeyValuePair<string, Glyph> pair in glyphs )
            {
                yield return pair.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        /// <summary>
        /// Size of glyphs in the dabase.
        /// </summary>
        /// 
        /// <remarks><para>All glyph of a database are <b>size</b>x<b>size</b> square glyph.</para></remarks>
        /// 
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Number of glyphs in the database.
        /// </summary>
        public int Count
        {
            get { return glyphs.Count; }
        }

        /// <summary>
        /// Get glyph by its name.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to retrieve for the database.</param>
        /// 
        /// <returns>Returns the glyph with the specified name or <see langword="null"/> if such
        /// glyph does not exist.</returns>
        /// 
        public Glyph this[string name]
        {
            get { return ( glyphs.ContainsKey( name ) ) ? glyphs[name] : null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphDatabase"/> class.
        /// </summary>
        /// 
        /// <param name="size"><see cref="Size">Size</see> of glyphs to store in the database.</param>
        /// 
        public GlyphDatabase( int size )
        {
            this.size = size;
        }

        /// <summary>
        /// Add glyph to the database.
        /// </summary>
        /// 
        /// <param name="glyph">Glyph to add to the database.</param>
        /// 
        /// <exception cref="ApplicationException">Glyph size does not match the database.</exception>
        /// 
        public void Add( Glyph glyph )
        {
            if ( glyph.Size != size )
            {
                throw new ApplicationException( "Glyph size does not match the database." );
            }

            glyphs.Add( glyph.Name, glyph );
        }

        /// <summary>
        /// Remove a glyph from the database.
        /// </summary>
        /// 
        /// <param name="name">Glyph name to remove from the database.</param>
        /// 
        public void Remove( string name )
        {
            if ( glyphs.ContainsKey( name ) )
                glyphs.Remove( name );
        }

        /// <summary>
        /// Replace a glyph in the databse.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to replace.</param>
        /// <param name="newGlyph">New glyph to put into the database.</param>
        /// 
        /// <remarks><para>If the specified glyph's <paramref name="name"/> equals to the <see cref="Glyph.Name">name of the new glyph</see>,
        /// then the database is just updated with the new glyph. But if these names are different, then the old glyph with the specified
        /// name is removed from the database and the new glyph is added.</para></remarks>
        /// 
        /// <exception cref="ArgumentException">A glyph with the specified <paramref name="name"/> does not exist in the database.</exception>
        /// 
        public void Replace( string name, Glyph newGlyph )
        {
            if ( !glyphs.ContainsKey( name ) )
            {
                throw new ArgumentException( "A glyph with the specified name does not exist in the database." );
            }

            if ( name == newGlyph.Name )
            {
                glyphs[name] = newGlyph;
            }
            else
            {
                Remove( name );
                Add( newGlyph );
            }
        }

        /// <summary>
        /// Rename a glyph in the database.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to rename.</param>
        /// <param name="newName">New name of the glyph to set.</param>
        /// 
        /// <exception cref="ArgumentException">A glyph with the specified <paramref name="name"/> does not exist in the database.</exception>
        /// 
        public void Rename( string name, string newName )
        {
            if ( !glyphs.ContainsKey( name ) )
            {
                throw new ArgumentException( "A glyph with the specified name does not exist in the database." );
            }

            if ( name == newName )
                return;

            Glyph glyph = glyphs[name];
            glyphs.Remove( name );

            glyph.Name = newName;
            glyphs.Add( newName, glyph );
        }

        /// <summary>
        /// Get collection of glyph names available in the database.
        /// </summary>
        /// 
        /// <returns>Returns read only collection of glyph names.</returns>
        /// 
        public ReadOnlyCollection<string> GetGlyphNames( )
        {
            return new ReadOnlyCollection<string>( new List<string>( glyphs.Keys ) );
        }

        /// <summary>
        /// Recognize the glyph represented by the specified raw glyph's data.
        /// </summary>
        /// 
        /// <param name="rawGlyphData">Raw glyph data to recognize.</param>
        /// <param name="rotation">Contains rotation angle of the match on success (0, 90, 180 or 270) -
        /// see <see cref="Glyph.CheckForMatching(byte[,])"/>. In the case of no matching is found the value is
        /// assigned to -1.</param>
        /// 
        /// <returns>Returns a glyph from the database which matches the specified raw glyph data. If there is
        /// no matching found, then <see langword="null"/> is returned.</returns>
        /// 
        /// <remarks><para>The method searches for a glyph in the database which matches (see <see cref="Glyph.CheckForMatching(byte[,])"/>)
        /// the specified raw glyph data.</para></remarks>
        /// 
        public Glyph RecognizeGlyph( byte[,] rawGlyphData, out int rotation )
        {
            foreach ( KeyValuePair<string, Glyph> pair in glyphs )
            {
                if ( ( rotation = pair.Value.CheckForMatching( rawGlyphData ) ) != -1 )
                    return pair.Value;
            }

            rotation = -1;
            return null;
        }
    }
}
