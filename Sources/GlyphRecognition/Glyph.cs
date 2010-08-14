
namespace AForge.Vision.GlyphRecognition
{
    using System;

    public class Glyph : ICloneable
    {
        private string name;

        private byte[,] data;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Size
        {
            get { return data.GetLength( 0 ); }
        }

        public byte[,] Data
        {
            get { return data; }
            set { data = value; }
        }

        public Glyph( string name, int size )
        {
            this.name = name;
            this.data = new byte[size, size];
        }

        public Glyph( string name, byte[,] data )
        {
            this.name = name;
            this.data = data;
        }

        public object Clone( )
        {
            return new Glyph( name, (byte[,]) data.Clone( ) );
        }

    }
}
