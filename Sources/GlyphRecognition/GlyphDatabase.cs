
namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class GlyphDatabase : IEnumerable<Glyph>
    {
        Dictionary<string, Glyph> glyphs = new Dictionary<string,Glyph>( );

        // size of glyphs in the database
        private int size;


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


        public int Size
        {
            get { return size; }
        }

        public int Count
        {
            get { return glyphs.Count; }
        }


        public GlyphDatabase( int size )
        {
            this.size = size;
        }

        public void Add( Glyph glyph )
        {
            if ( glyph.Size != size )
            {
                throw new ApplicationException( "Glyph size does not match the database" );
            }

            glyphs.Add( glyph.Name, glyph );
        }

        public void Remove( string name )
        {
        }

        public void Rename( string name, string newName )
        {
            if ( name == newName )
                return;
        }
    }
}
