﻿using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKPrimitives
{
    public class RenderObject
    {
        public struct Vertex
        {
            public const int Size = (4 + 4) * 4; // size of struct in bytes

            private  Vector4 _position;
            private  Color4 _color;

            public Vertex(Vector4 position, Color4 color)
            {
                _position = position;
                _color = color;
            }
        }
        // size of struct in bytes

        private bool _initialized;
        private readonly int _vertexArray;
        private readonly int _buffer;
        private readonly int _verticeCount;

        public RenderObject(Vertex[] vertices)
        {
            _verticeCount = vertices.Length;

            _vertexArray = GL.GenVertexArray();
            _buffer = GL.GenBuffer();
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArray);
            GL.NamedBufferStorage(
              _buffer,
              Vertex.Size * vertices.Length,        // the size needed by this buffer
              vertices,                           // data to initialize with
              BufferStorageFlags.MapWriteBit);    // at this point we will only write to the buffer
            GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArray, 0);
            GL.VertexArrayAttribFormat(
                _vertexArray,
                0,                      // attribute index, from the shader location = 0
                4,                      // size of attribute, vec4
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                     // relative offset, first item
            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribFormat(
                _vertexArray,
                1,                      // attribute index, from the shader location = 1
                4,                      // size of attribute, vec4
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                     // relative offset after a vec4

            GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, Vertex.Size);
            _initialized = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
   
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(_vertexArray);
                    GL.DeleteBuffer(_buffer);
                    _initialized = false;
                }
            }
        }
        public void Bind()
        {
            GL.BindVertexArray(_vertexArray);
        }
        public void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticeCount);
        }
    }
}
