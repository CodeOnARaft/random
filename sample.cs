using Raylib_cs;
using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

class Program
{
    // Constants
    const int CELLSIZE = 16;
    const int CELLCOUNT = 50;
    static Color[] colors = { Color.Brown, Color.DarkGreen, Color.Gray };

    static void Main()
    {

        int playerX = 100;
        int xspeed = 3;
        int playerY = 100;
        float velocity = 0.0f;

        Raylib.InitAudioDevice();
        Raylib.InitWindow(CELLCOUNT * CELLSIZE, CELLCOUNT * CELLSIZE, "terara");
        Raylib.SetTargetFPS(60);

        Color[,] colormatrix = new Color[CELLCOUNT, CELLCOUNT];
        Rectangle[,] BlockRectangles = new Rectangle[CELLCOUNT, CELLCOUNT];

        InitBoard(colormatrix, BlockRectangles);

        while (!Raylib.WindowShouldClose())

        {

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Rectangle playerRect = new Rectangle(playerX, playerY, CELLSIZE, CELLSIZE);

            float frameTime = Raylib.GetFrameTime();          
          

            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                playerRect.X += xspeed;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                playerRect.X -= xspeed;
            }

            var collision = TestCollisions(BlockRectangles, playerRect);
            if (!collision)
            {
                playerX = (int)playerRect.X;               
            }
            else
            {
                playerRect.X = playerX;                 
            }

            velocity = velocity < 0 ? velocity + frameTime : Math.Max(1,velocity + frameTime);
            playerRect.Y = (float)Math.Floor( playerY + velocity);
            collision = TestCollisions(BlockRectangles, playerRect);
            Console.WriteLine($"{collision}");
            if (!collision)
            {
                playerY = (int)playerRect.Y;
            }
            else
            {
                velocity = 0;
                playerRect.Y = playerY;
                if (Raylib.IsKeyReleased(KeyboardKey.Space))
                {
                    velocity -= 1; // TODO this instant jump should be done over frametime 
                }
            }

            TestMouseInput(colormatrix, BlockRectangles);

            for (int x = 0; x < CELLCOUNT; x++)
            {

                for (int y = 0; y < CELLCOUNT; y++)
                {
                    Raylib.DrawRectangle(x * CELLSIZE, y * CELLSIZE, CELLSIZE, CELLSIZE, colormatrix[x, y]);
                }

            }

            Raylib.DrawRectangleRec(playerRect, Color.Red);
            Raylib.EndDrawing();

        }

        Raylib.CloseWindow();

        Raylib.CloseAudioDevice();

    }

    private static bool TestCollisions(Rectangle[,] blockRectangles, Rectangle playerRect)
    {
        for (int x = 0; x < CELLCOUNT; x++)
        {
            for (int y =0; y < CELLCOUNT; y++)
            {
                if (Raylib.CheckCollisionRecs(playerRect, blockRectangles[x, y]))
                {
                    return true;
                }
            }
       
        }
        return false;
    }

    private static void TestMouseInput(Color[,] colormatrix, Rectangle[,] BlockRectangles)
    {
            int mousePosX = Raylib.GetMouseX() / CELLSIZE;
            int mousePosY = Raylib.GetMouseY() / CELLSIZE;

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {

            colormatrix[mousePosX, mousePosY] = Color.SkyBlue;
            BlockRectangles[mousePosX, mousePosY] = new Rectangle(0, 0, 0, 0);

        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Right))

        {
            colormatrix[mousePosX, mousePosY] = Color.Brown;
            BlockRectangles[mousePosX, mousePosY] = new Rectangle(mousePosX * CELLSIZE, mousePosY * CELLSIZE, CELLSIZE, CELLSIZE);
        }
    }

    private static void InitBoard(Color[,] colormatrix, Rectangle[,] BlockRectangles)
    {
        
        for (int x = 0; x < CELLCOUNT; x++)

        {

            int surfaceY = Raylib.GetRandomValue(20, 23);

            for (int y = 0; y < CELLCOUNT; y++)

            {

                if (y < surfaceY)

                {

                    colormatrix[x, y] = Color.SkyBlue;

                    BlockRectangles[x, y] = new Rectangle(0, 0, 0, 0);

                }

                else if (y == surfaceY)

                {

                    colormatrix[x, y] = Color.Green;

                    BlockRectangles[x, y] = new Rectangle(x * CELLSIZE, y * CELLSIZE, CELLSIZE, CELLSIZE);

                }

                else if (y > surfaceY && y < surfaceY + 5)

                {

                    colormatrix[x, y] = Color.Brown;

                    BlockRectangles[x, y] = new Rectangle(x * CELLSIZE, y * CELLSIZE, CELLSIZE, CELLSIZE);

                }

                else

                {

                    colormatrix[x, y] = Color.Gray;

                    BlockRectangles[x, y] = new Rectangle(x * CELLSIZE, y * CELLSIZE, CELLSIZE, CELLSIZE);

                }

            }

        }

    }
}
