using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using OpenGL;
using System.Drawing;


namespace First_App_Prim
{
   class Program
    {
        private static int Width = 600;
        private static int Height = 398;
        private static ShaderProgram program, program_cyrcle, program_cylindre;
        private static VBO<Vector3> panel, corner, cross, cyrcle, sphere;
        private static VBO<Vector2> panelUV, cornerUV, crossUV, cyrcleUV, sphereUV;
        private static VBO<uint> panelTriangles, cornerTriangles, crossTriangles, cyrcleTriangles, sphereTriangles;
        private static Matrix4 ViewMatrix;
        private static Matrix4 ProjectionMatrix;
        private static Texture ChessboardTexrute, panelTexture, cornerTexture, crossTexture, cyrcleTexture, cylindrBodyTexture, sphereTexture;
        private static float a = 1;
        private static float n = 200;
        static void Main(string[] args)
        {
            // Инициализируем главное окно
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(Width, Height);
            Glut.glutCreateWindow("OpenGL Ex. #44");
            Gl.Viewport(0, 0, Width, Height);

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutCloseFunc(OnClose);

            Gl.Enable(EnableCap.Blend);
            Gl.Enable(EnableCap.DepthTest);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Gl.DepthFunc(DepthFunction.Less);
            Gl.ClearColor(0.44f, 0.49f, 0.527f, 1);

            program = new ShaderProgram(VertexShader, FragmentShader);
            program_cyrcle = new ShaderProgram(VertexShader_Cyrcle, FragmentShader);
            program_cylindre = new ShaderProgram(VertexShader_Cylinder, FragmentShader);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(0.41f, (float)Width / Height, 0.1f, 1000f);
            ViewMatrix = Matrix4.LookAt(new Vector3(32.5f, 10.8f, -54.8f), new Vector3(13.4f, 0.7f, -3.0f), new Vector3(0, 1, 0));

            program.Use();
            program["projection_matrix"].SetValue(ProjectionMatrix);
            program["view_matrix"].SetValue(ViewMatrix);

            program_cyrcle.Use();
            program_cyrcle["projection_matrix"].SetValue(ProjectionMatrix);
            program_cyrcle["view_matrix"].SetValue(ViewMatrix);

            program_cylindre.Use();
            program_cylindre["projection_matrix"].SetValue(ProjectionMatrix);
            program_cylindre["view_matrix"].SetValue(ViewMatrix);

            ChessboardTexrute = loadTexture("ChessBoard.png");
            panelTexture = loadTexture("panel.png");
            cornerTexture = loadTexture("corner.png");
            crossTexture = loadTexture("cross.png");
            cyrcleTexture = loadTexture("cyrcle.png");
            cylindrBodyTexture =  loadTexture("Wood.jpg");
            sphereTexture = loadTexture("Earth.jpg");

            panel = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 1.0f)
            });
            panelUV = new VBO<Vector2>(new Vector2[]  {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            });
            panelTriangles = new VBO<uint>(new uint[] {
                0, 1, 2,
                0, 2, 3
            }, BufferTarget.ElementArrayBuffer);

            corner = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, 0.5f),
                new Vector3(0.5f, 0.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 1.0f)
            });
            cornerUV = new VBO<Vector2>(new Vector2[]  {
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 1.0f),
                new Vector2(0.0f, 1.0f)
            });
            cornerTriangles = new VBO<uint>(new uint[] {
                0, 1, 5,
                1, 2, 3,
                3, 4, 5
            }, BufferTarget.ElementArrayBuffer);

            cross = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.0f,   0.0f, 0.0f),
                new Vector3(0.25f,  0.0f, 0.0f),
                new Vector3(0.5f,   0.0f, 0.25f),
                new Vector3(0.75f,  0.0f, 0.0f),
                new Vector3(1.0f,   0.0f, 0.0f),
                new Vector3(1.0f,   0.0f, 0.25f),
                new Vector3(0.75f,  0.0f, 0.5f),
                new Vector3(1.0f,   0.0f, 0.75f),
                new Vector3(1.0f,   0.0f, 1.0f),
                new Vector3(0.75f,  0.0f, 1.0f),
                new Vector3(0.5f,   0.0f, 0.75f),
                new Vector3(0.25f,  0.0f, 1.0f),
                new Vector3(0.0f,   0.0f, 1.0f),
                new Vector3(0.0f,   0.0f, 0.75f),
                new Vector3(0.25f,  0.0f, 0.5f),
                new Vector3(0.0f,   0.0f, 0.25f)
            });
            crossUV = new VBO<Vector2>(new Vector2[]  {
                new Vector2(0.0f,  0.0f),
                new Vector2(0.25f, 0.0f),
                new Vector2(0.5f,  0.25f),
                new Vector2(0.75f, 0.0f),
                new Vector2(1.0f,  0.0f),
                new Vector2(1.0f,  0.25f),
                new Vector2(0.75f, 0.5f),
                new Vector2(1.0f,  0.75f),
                new Vector2(1.0f,  1.0f),
                new Vector2(0.75f, 1.0f),
                new Vector2(0.5f,  0.75f),
                new Vector2(0.25f, 1.0f),
                new Vector2(0.0f,  1.0f),
                new Vector2(0.0f,  0.75f),
                new Vector2(0.25f, 0.5f),
                new Vector2(0.0f,  0.25f)
            });
            crossTriangles = new VBO<uint>(new uint[] {
                0, 1, 15,
                1, 2, 15,
                2, 14, 15,
                3, 6, 13,
                3, 4, 5,
                3, 5, 6,
                11, 12, 13,
                6, 11, 13,
                6, 9, 10,
                7, 8, 9,
                6, 7, 9
            }, BufferTarget.ElementArrayBuffer);

            cyrcle = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(0.5f, 0.0f, 0.5f),
            });
            cyrcleUV = new VBO<Vector2>(new Vector2[]  {
                new Vector2(0.5f, 0.5f),
                new Vector2(1.0f, 0.5f),
                new Vector2(0,0)
            });
            cyrcleTriangles = new VBO<uint>(new uint[] {
                0, 1, 2
            }, BufferTarget.ElementArrayBuffer);

            float angle = (float)((Math.PI) / n);

            List<Vector3> sphere_vertex = new List<Vector3>();
            List<Vector2> sphere_to = new List<Vector2>();
            List<uint> sphere_indeses = new List<uint>();

            sphere_vertex.Add(new Vector3(0, 1, 0));
            sphere_to.Add(new Vector2(0, 1));
            sphere_vertex.Add(new Vector3((float)Math.Cos(Math.PI / 2 - angle), (float)Math.Sin(Math.PI / 2 - angle), 0));
            sphere_to.Add(new Vector2(0, 1.0f - 1.0f / n));
            sphere_vertex.Add(Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle) * sphere_vertex[sphere_vertex.Count - 1]);
            sphere_to.Add(new Vector2(1.0f / (2 * n), 1.0f - 1.0f / n));
            sphere_indeses.Add(0);
            sphere_indeses.Add(1);
            sphere_indeses.Add(2);

            for (uint i = 2; i < n; i++)
            {
                sphere_vertex.Add(new Vector3((float)Math.Cos(Math.PI / 2 - angle * i), (float)Math.Sin(Math.PI / 2 - angle * i), 0));
                sphere_to.Add(new Vector2(0, 1.0f - i / n));
                sphere_vertex.Add(Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle) * sphere_vertex[sphere_vertex.Count - 1]);
                sphere_to.Add(new Vector2(1.0f / (2 * n), 1.0f - i / n));
                sphere_indeses.Add(2*i - 3);
                sphere_indeses.Add(2*i - 2);
                sphere_indeses.Add(2*i - 1);
                sphere_indeses.Add(2 * i - 2);
                sphere_indeses.Add(2 * i - 1);
                sphere_indeses.Add(2 * i);
            }

            sphere_vertex.Add(new Vector3(0, -1, 0));
            sphere_to.Add(new Vector2(0, 0));
            sphere_indeses.Add(2 * (uint)n - 3);
            sphere_indeses.Add(2 * (uint)n - 2);
            sphere_indeses.Add(2 * (uint)n - 1);

            sphere = new VBO<Vector3>(sphere_vertex.ToArray());
            sphereUV = new VBO<Vector2>(sphere_to.ToArray());
            sphereTriangles = new VBO<uint>(sphere_indeses.ToArray(), BufferTarget.ElementArrayBuffer);


            Glut.glutMainLoop();
        }
        private static Texture loadTexture(string name)
        {
            Texture newTexture = new Texture(name);

            Gl.BindTexture(newTexture);
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureParameter.Repeat);
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureParameter.Repeat);

            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.Linear);
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Linear);

            int[] g_nMaxAnisotropy = { 0 };
            Gl.GetIntegerv(GetPName.MaxTextureMaxAnisotropyExt, g_nMaxAnisotropy);
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.MaxAnisotropyExt,
                    g_nMaxAnisotropy[0]);
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Linear);

            return newTexture;
        }
        private static void OnClose()
        {
   
            panel.Dispose();
            panelUV.Dispose();
            panelTriangles.Dispose();
            ChessboardTexrute.Dispose();
            program.DisposeChildren = true;
            program.Dispose();
        }

        private static void OnDisplay()
        {

        }

        private static void OnRenderFrame()
        { 
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            a += 0.008f;
            DrawChessBoard(0, 0, 0, 90.6f, 90.6f, ChessboardTexrute);                                 //Chessboard
            DrawBox(21.5f, 0.0f, -18.92f, 5.73f, 2.2f, 5.32f, Matrix4.Identity, panelTexture);        //1
            Draw3Corner(26.54f, 2.3f, -18.38f, 4.1f, 5.1f, 5.0f,                                      //2
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 2),                      //
                panelTexture, cornerTexture);                                                         //
            DrawBox(22.03f, 0.0f, -7.39f, 5.12f, 1.9f, 4.51f, Matrix4.Identity, panelTexture);        //3
            DrawBox(23.9f, 1.9f, -6.85f, 2.87f, 3.89f, 2.05f, Matrix4.Identity, panelTexture);        //4
            DrawBox(13.6f, 0.0f, -18.46f, 4.3f, 2.1f, 4.51f, Matrix4.Identity, panelTexture);         //5
            DrawBox(14.5f, 2.1f, -17.82f, 3.0f, 3.4f, 2.87f, Matrix4.Identity, panelTexture);         //6
            DrawBox(12.91f, 0.0f, -6.76f, 4.7f, 1.9f, 4.51f, Matrix4.Identity, panelTexture);         //7
            DrawBox(23.14f, 0.0f, 23.14f, 4.71f, 1.64f, 4.5f, Matrix4.Identity, panelTexture);        //8
            DrawBox(22.84f, 1.64f, 24.24f, 4.51f, 3.8f, 3.28f, Matrix4.Identity, panelTexture);       //9
            DrawBox(25.6f, 0.0f, 31.33f, 3.69f, 1.8f, 5.32f, Matrix4.Identity, panelTexture);         //10
            Draw3Corner(27.85f, 1.8f, 32.97f, 4.1f, 5.1f, 5.0f,                                       //11
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI),                           //
                panelTexture, cornerTexture);                                                         //
            DrawCross(12.3f, 1.9f, -6.2f, 4.8f, 5f, 3.3f,                                             //12
                Matrix4.Identity,                                                                     //
                panelTexture, crossTexture);                                                          //
            DrawCross(12.3f, 1.9f, 23.2f, 4.8f, 5f, 3.3f,                                             //13
                Matrix4.Identity,                                                                     //
                panelTexture, crossTexture);                                                          //
            DrawBox(7.5f, 0.0f, 31.33f, 3.69f, 6.3f, 5.32f, Matrix4.Identity, panelTexture);          //14
            DrawBox(13.2f, 0.0f, 31.33f, 3.69f, 5.5f, 5.32f, Matrix4.Identity, panelTexture);         //15
            DrawBox(2.9f, 0.0f, 1.3f, 4.7f, 1.9f, 4.51f, Matrix4.Identity, panelTexture);             //16
            DrawBox(3.6f, 1.9f, 2.1f, 4.9f, 5.0f, 3.3f, Matrix4.Identity, panelTexture);              //17
            DrawBox(4.0f, 6.9f, 3.5f, 4.0f, 3.0f, 2.7f,                                               //18
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI/4), panelTexture);          //
            DrawBox(3.6f, 1.7f, 12.8f, 4.9f, 5.0f, 3.3f, Matrix4.Identity, panelTexture);             //19
            DrawBox(4.0f, 6.7f, 14.2f, 4.0f, 3.0f, 2.7f,                                              //20
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI / 4), panelTexture);        //
            DrawBox(-7.4f, 0.0f, 1.3f, 5.7f, 1.9f, 6.5f, Matrix4.Identity, panelTexture);             //21
            DrawCylinder(-4.55f, 1.9f, 4.55f, 2.35f, 5, 30,                                           //22
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 2),                      //
                cyrcleTexture, panelTexture);                                                         //
            DrawBox(-7.4f, 0.0f, 11.0f, 5.7f, 1.9f, 6.5f, Matrix4.Identity, panelTexture);            //23
            DrawCylinder(-4.55f, 1.9f, 13.95f, 2.35f, 5, 30,                                          //24
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 2),                      //
                cyrcleTexture, panelTexture);                                                         //
            DrawSphere(-4.55f, 8.5f, 4.55f, 1.9f, n,                                                  //25
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI), sphereTexture);          //
            DrawSphere(-4.55f, 8.5f, 14.1f, 1.9f, n,                                                  //26
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI), sphereTexture);          //
            DrawBox(-23f, 0.0f, 20.2f, 4.3f, 2.1f, 4.51f, Matrix4.Identity, panelTexture);            //27
            DrawBox(-22f, 2.1f, 20.2f, 3.0f, 3.4f, 2.87f, Matrix4.Identity, panelTexture);            //28
            DrawBox(-22f, 2.1f, 25.2f, 5.5f, 4.5f, 2.87f, Matrix4.Identity, panelTexture);            //28

            //DrawSphere(20, 5, -30, 3, n, Matrix4.CreateFromAxisAngle(Vector3.UnitY, -a), sphereTexture);
            Glut.glutSwapBuffers();
        }

        private static void DrawChessBoard(float centerX, float centerY, float CenterZ, float width_x, float depth_z, Texture texture)
        {
            Gl.UseProgram(program);
            Gl.BindTexture(texture);
            Gl.BindBufferToShaderAttribute(panel, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(panelUV, program, "vertexUV");
            Gl.BindBuffer(panelTriangles);
            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width_x, 1, depth_z)) * Matrix4.CreateTranslation(new Vector3(centerX - width_x/2, centerY, CenterZ - depth_z/2)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private static void DrawBox(float x, float y, float z, float width, float height, float depth, Matrix4 quaternion,Texture texture)
        {
            Gl.UseProgram(program);
            Gl.BindTexture(texture);
            Gl.BindBufferToShaderAttribute(panel, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(panelUV, program, "vertexUV");
            Gl.BindBuffer(panelTriangles);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width, height, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, 0, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3( width, 0, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private static void Draw3Corner(float x, float y, float z, float width, float height, float depth, Matrix4 quaternion, Texture texture_panel, Texture texture_corner)
        {
            //quaternion = Matrix4.Identity;

            Gl.UseProgram(program);


            Gl.BindTexture(texture_panel);
            Gl.BindBufferToShaderAttribute(panel, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(panelUV, program, "vertexUV");
            Gl.BindBuffer(panelTriangles);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width / 2, height, depth / 2)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(0,  height / 2,  depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width / 2, height, depth / 2)) * Matrix4.CreateTranslation(new Vector3(0,  height / 2, depth / 2)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(0, 0, depth / 2)) * quaternion  * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(width / 2,  height, depth / 2)) * quaternion *  Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(width / 2, height / 2, 0)) *quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width / 2, height, depth / 2)) * Matrix4.CreateTranslation(new Vector3( width / 2, height / 2, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(width / 2, height / 2, depth / 2)) * quaternion  * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 2, height / 2, depth / 2)) * Matrix4.CreateTranslation(new Vector3(width, height, 0)) * quaternion *  Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero); Gl.BindTexture(texture_panel);

            Gl.BindTexture(texture_corner);
            Gl.BindBufferToShaderAttribute(corner, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(cornerUV, program, "vertexUV");
            Gl.BindBuffer(cornerTriangles);

            program["model_matrix"].SetValue( Matrix4.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, cornerTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, cornerTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * quaternion *  Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, cornerTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private static void DrawCross(float x, float y, float z, float width, float height, float depth, Matrix4 quaternion, Texture texture_panel, Texture texture_cross)
        {

            Gl.UseProgram(program);
            Gl.BindTexture(texture_panel);
            Gl.BindBufferToShaderAttribute(panel, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(panelUV, program, "vertexUV");
            Gl.BindBuffer(panelTriangles);

            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(0, 0, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f*width / 4, height, 1.414f*depth / 4)) * Matrix4.CreateTranslation(new Vector3(width/4, 0, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(width / 2, 0, depth*0.75f)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(0.75f*width, 0, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(width, 0, 0.75f*depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(0.75f*width, 0, depth * 0.5f)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(width* 0.75f, 0, depth*0.5f)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(width, 0, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(0.75f * width, 0, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(width / 2, 0, depth/4)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(width / 4, 0, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(0, 0, depth * 0.25f)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)Math.PI / 2) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)Math.PI / 4) * Matrix4.CreateScaling(new Vector3(1.414f * width / 4, height, 1.414f * depth / 4)) * Matrix4.CreateTranslation(new Vector3(0, 0, depth * 0.75f)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2) * Matrix4.CreateScaling(new Vector3(width / 4, height, depth / 4)) * Matrix4.CreateTranslation(new Vector3(0, 0, 0.75f * depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Gl.UseProgram(program);
            Gl.BindTexture(texture_cross);
            Gl.BindBufferToShaderAttribute(cross, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(crossUV, program, "vertexUV");
            Gl.BindBuffer(crossTriangles);

            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width, height, depth)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, crossTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(width, height, depth)) * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            Gl.DrawElements(BeginMode.Triangles, crossTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private static void DrawCylinder(float x, float y, float z,float r, float h, float n, Matrix4 quaternion, Texture texture_cyrcle, Texture body)
        {
            Gl.UseProgram(program_cyrcle);
            Gl.BindTexture(texture_cyrcle);
            Gl.BindBufferToShaderAttribute(cyrcle, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(cyrcleUV, program, "vertexUV");
            Gl.BindBuffer(cyrcleTriangles);
            float angle = (float)((Math.PI * 2) / n);
            program_cyrcle["segment_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle));
            program_cyrcle["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(r, r, r)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            for (int i = 0; i < n; i++)
            {
                program_cyrcle["rotate_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle * i));
                Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
            program_cyrcle["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(r, r, r)) * Matrix4.CreateTranslation(new Vector3(0, h, 0)) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
            for (int i = 0; i < n; i++)
            {
                program_cyrcle["rotate_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle*i));
                Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
            Gl.UseProgram(program_cylindre);
            Gl.BindTexture(body);
            Gl.BindBufferToShaderAttribute(panel, program_cylindre, "vertexPosition");
            Gl.BindBufferToShaderAttribute(panelUV, program_cylindre, "vertexUV");
            Gl.BindBuffer(panelTriangles);
            float len = (float)Math.Sqrt((double)(2 * r * r - 2 * r * r * Math.Cos((double)angle)));
            for (int i = 0; i < n; i++)
            {
                program_cylindre["ind"].SetValue(new Vector2(i, n));
                program["model_matrix"].SetValue(Matrix4.CreateFromAxisAngle(Vector3.UnitX, -(float)(Math.PI / 2)) * Matrix4.CreateScaling(new Vector3(len, h, 0)) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float)(Math.PI / 2) + angle /2) * Matrix4.CreateTranslation(new Vector3(r, 0, 0)) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, angle * i) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
                Gl.DrawElements(BeginMode.Triangles, panelTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

        }

        private static void DrawSphere(float x, float y, float z, float r, float n, Matrix4 quaternion, Texture texture)
        {


            float angle = (float)((Math.PI) / n);

            Gl.UseProgram(program_cylindre);
            Gl.BindTexture(texture);
            Gl.BindBufferToShaderAttribute(sphere, program_cylindre, "vertexPosition");
            Gl.BindBufferToShaderAttribute(sphereUV, program_cylindre, "vertexUV");
            Gl.BindBuffer(sphereTriangles);

            for (int i = 0; i < 2*n; i++)
            {
                program_cylindre["ind"].SetValue(new Vector2(i, 2*n));
                program["model_matrix"].SetValue(Matrix4.CreateScaling(new Vector3(r, r, r))*Matrix4.CreateFromAxisAngle(Vector3.UnitY, -angle * i) * quaternion * Matrix4.CreateTranslation(new Vector3(x, y, z)));
                Gl.DrawElements(BeginMode.Triangles, sphereTriangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
        }

        public static string VertexShader = @"
        #version 130
        in vec3 vertexPosition;
        in vec2 vertexUV;
        out vec2 uv;
        uniform mat4 projection_matrix;
        uniform mat4 view_matrix;
        uniform mat4 model_matrix;
        void main(void)
        {
            uv = vertexUV;
            gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
        }";

        public static string FragmentShader = @"
        #version 130
        uniform sampler2D texture;
        in vec2 uv;
        out vec4 fragment;
        void main(void)
        {
                fragment = texture2D(texture, uv);
        }";

        public static string VertexShader_Cyrcle = @"
        #version 130
        in vec3 vertexPosition;
        in vec2 vertexUV;
        out vec2 uv;
        uniform mat4 segment_matrix;
        uniform mat4 rotate_matrix;
        uniform mat4 projection_matrix;
        uniform mat4 view_matrix;
        uniform mat4 model_matrix;
        void main(void)
        {
            uv = vertexUV;
            if (vertexPosition.x == 0.5){
                gl_Position = projection_matrix * view_matrix * model_matrix * rotate_matrix * segment_matrix * vec4(1,0,0,1);
                vec4 tempVec =  ((rotate_matrix * segment_matrix * vec4(0.5,0,0,1)) + vec4(0.5, 0, 0.5, 0));
                uv =  vec2(tempVec.x, tempVec.z);
            }
            else {
                vec4 tempVec = ((rotate_matrix * (vec4(vertexUV.x,0,vertexUV.y,1) - vec4(0.5, 0, 0.5, 0))) + vec4(0.5, 0, 0.5, 0));
                uv = vec2(tempVec.x, tempVec.z);
                gl_Position = projection_matrix * view_matrix * model_matrix * rotate_matrix * vec4(vertexPosition, 1);
            }   
        }";

        public static string VertexShader_Cylinder = @"
        #version 130
        in vec3 vertexPosition;
        in vec2 vertexUV;
        out vec2 uv;
        uniform vec2 ind;
        uniform mat4 projection_matrix;
        uniform mat4 view_matrix;
        uniform mat4 model_matrix;
        void main(void)
        {
            gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
            uv = vec2(smoothstep(0, ind.y, ind.x + vertexUV.x),vertexUV.y);
        }";

    }

}