using System;
using SplashKitSDK;

public abstract class Robot
{
    public double X { get; set; }
    public double Y { get; set; }
    public Color MainColor { get; set; }
    private Vector2D Velocity { get; set; }

    public Circle CollisionCircle
    {
        get { return SplashKit.CircleAt(X, Y, 20); }
    }

    public int Width => 50;
    public int Height => 50;

    public Robot(Window gameWindow, Player player)
    {
        if (SplashKit.Rnd() < 0.5)
        {
            X = SplashKit.Rnd(gameWindow.Width);

            if (SplashKit.Rnd() < 0.5)
            {
                Y = -Height;
            }
            else
            {
                Y = gameWindow.Height;
            }
        }
        else
        {
            Y = SplashKit.Rnd(gameWindow.Height);

            if (SplashKit.Rnd() < 0.5)
            {
                X = -Width;
            }
            else
            {
                X = gameWindow.Width;
            }
        }

        const int SPEED = 4;

        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };

        Point2D toPt = new Point2D()
        {
            X = player.X,
            Y = player.Y
        };

        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

        Velocity = SplashKit.VectorMultiply(dir, SPEED);

        MainColor = Color.RandomRGB(200);
    }

    public abstract void Draw();

    public bool IsOffscreen(Window screen)
    {
        return (X < -Width || X > screen.Width || Y < -Height || Y > screen.Height);
    }

    public void Update()
    {
        X = X + Velocity.X;
        Y = Y + Velocity.Y;
    }
}

public class Boxy : Robot
{
    public Boxy(Window gameWindow, Player player) : base(gameWindow, player)
    {
        // Additional initialization for Boxy if needed
    }

    public override void Draw()
    {
        double leftX = X + 12;
        double rightX = X + 27;
        double eyeY = Y + 10;
        double mouthY = Y + 30;

        SplashKit.FillRectangle(Color.Gray, X, Y, Width, Height);
        SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
        SplashKit.FillRectangle(MainColor, leftX + 2, mouthY + 2, 21, 6);
    }
}

public class Roundy : Robot
{
    public Roundy(Window gameWindow, Player player) : base(gameWindow, player)
    {
        // Additional initialization for Roundy if needed
    }

    public override void Draw()
    {
        double midX = X + 25;
        double midY = Y + 25;

        SplashKit.FillCircle(Color.White, midX, midY, 25);
        SplashKit.DrawCircle(Color.Gray, midX, midY, 25);
        SplashKit.FillCircle(MainColor, midX + 7, midY - 5, 5);
        SplashKit.FillCircle(MainColor, midX - 7, midY - 5, 5);
        SplashKit.FillEllipse(Color.Gray, X, Y + 20, Width, Height - 20);
        SplashKit.DrawLine(Color.Black, X, Y + 35, X + Width, Y + 35);
    }
}
