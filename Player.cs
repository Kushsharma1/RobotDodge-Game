using SplashKitSDK;
using System;

public class Player
{
    private Bitmap _playerBitmap;

    public double X { get; private set; }
    public double Y { get; private set; }

    public int Lives { get; set; } 
    public bool Quit { get; private set; }
    public int Score { get; set; }

    private bool _spacePressed;
    private Window _gameWindow;

    public int Width => _playerBitmap.Width;

    public int Height => _playerBitmap.Height;

    public Player(Window window)
    {
        _playerBitmap = new Bitmap("Player", "Player.png");
        Quit = false;
        _gameWindow = window;
        X = (_gameWindow.Width - Width) / 2;
        Y = (_gameWindow.Height - Height) / 2;
        
        Lives = 5;
    }

    public void Draw()
    {
        if (!_spacePressed)
        {
            SplashKit.ProcessEvents();
            _gameWindow.Clear(Color.LightBlue);

            _gameWindow.DrawText("Welcome to the Robot Dodge Game", Color.Black, _gameWindow.Width / 5, _gameWindow.Height / 6);
            _gameWindow.DrawText("Created By Kush Sharma", Color.Black, _gameWindow.Width / 5, _gameWindow.Height / 4);

            // Display menu within a box
            int menuBoxX = _gameWindow.Width / 6;
            int menuBoxY = _gameWindow.Height / 3;
            _gameWindow.FillRectangle(Color.White, menuBoxX, menuBoxY, 350, 80);
            _gameWindow.DrawText("1. Press Space Key to start playing", Color.Black, menuBoxX + 10, menuBoxY + 30);
            _gameWindow.DrawText("2. Press Esc Key to end the game!!", Color.Black, menuBoxX + 10, menuBoxY + 50);

            _playerBitmap.Draw(X, Y);
            _gameWindow.Refresh(60);
        }
        else
        {
            SplashKit.ProcessEvents();
            SplashKit.DrawBitmap(_playerBitmap, X, Y);
        }
    }

     public void HandleInput()
    {
        SplashKit.ProcessEvents();
        const int speed = 5;
        if (SplashKit.KeyDown(KeyCode.SpaceKey))
        {
            _spacePressed = true;
        }
        if (_spacePressed)
        {
            if (SplashKit.KeyDown(KeyCode.EscapeKey))
            {
                _gameWindow.Clear(Color.LightGreen);
                _gameWindow.DrawText("Thank you for playing. See you soon! :)", Color.Black, _gameWindow.Width /4, _gameWindow.Height / 2);
                _gameWindow.Refresh(60);
                SplashKit.Delay(3000);
                Quit = true;
            }
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                Y -= speed;
            }
            if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                Y += speed;
            }
            if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                X -= speed;
            }
            if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                X += speed;
            }
        }
    }

    public void StayOnWindow(Window limitWindow)
    {
        SplashKit.ProcessEvents();
        const int GAP = 10;

        if (X > limitWindow.Width - Width - GAP)
        {
            X = limitWindow.Width - Width - GAP;
        }
        if (Y > limitWindow.Height - Height - GAP)
        {
            Y = limitWindow.Height - Height - GAP;
        }
        if (Y < GAP)
        {
            Y = GAP;
        }
        if (X < GAP)
        {
            X = GAP;
        }
    }

    public bool CollidedWith(Robot robot)
    {
        return _playerBitmap.CircleCollision(X, Y, robot.CollisionCircle);
    }
}
