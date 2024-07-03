using System;
using SplashKitSDK;
using System.IO;
using System.Collections.Generic;

public class RobotDodge
{
    private Player _Player;
    private Window _GameWindow;
    private List<Robot> _Robots = new List<Robot>();
    private List<Robot> _removedRobots = new List<Robot>();
    private List<Bullet> _Bullets = new List<Bullet>();
    private List<Bullet> _removedBullets = new List<Bullet>();
    private SplashKitSDK.Timer myTimer;
    private Bitmap HeartBitmap = new Bitmap("Heart", "heart.png");
    

    public bool Quit
    {
        get
        {
            return _Player.Quit;
        }
    }

    public RobotDodge(Window window)
    {
        _GameWindow = window;
        _Player = new Player(window);
        myTimer = new SplashKitSDK.Timer("My Timer");
        myTimer.Start();
    }

    public void HandleInput()
    {
        _Player.HandleInput();
        _Player.StayOnWindow(_GameWindow);
    }

    public void Draw()
    {
        _GameWindow.Clear(Color.Beige);
        foreach (Robot robot in _Robots)
        {
            robot.Draw();
        }
        _Player.Draw();
        foreach (Bullet bullet in _Bullets)
        {
            bullet.Draw();
        }
        DrawHUD();
        if (_Player.Lives <= 0)
        {
            _GameWindow.Clear(Color.LightGreen);
            _GameWindow.DrawText("Oop! You lost all the lives :(", Color.Black, _GameWindow.Width / 1, _GameWindow.Height / 1);
            _GameWindow.DrawText("Game Over!", Color.Black, _GameWindow.Width / 2, _GameWindow.Height / 2);
            _GameWindow.DrawText("Press ESC to exit!", Color.Black, _GameWindow.Width / 3, _GameWindow.Height / 3);
        }
        _GameWindow.Refresh(60);
    }

    public void Update()
    {
        SplashKit.ProcessEvents();
        CheckCollisions();
        _Player.Score = Convert.ToInt32(myTimer.Ticks / 1000);
        foreach (Robot robot in _Robots)
        {
            robot.Update();
        }
        double randomNumber = SplashKit.Rnd(1000);
        if (randomNumber < 20)
        {
            _Robots.Add(RandomRobot());
        }
        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            _Bullets.Add(AddBullet());
        }
        foreach (Bullet bullet in _Bullets)
        {
            bullet.Update();
        }
    }

    public Robot RandomRobot()
{
    Robot randomRobot;

    if (SplashKit.Rnd() < 0.5)
    {
        randomRobot = new Boxy(_GameWindow, _Player);
    }
    else
    {
        randomRobot = new Roundy(_GameWindow, _Player);
    }

    return randomRobot;
}

    public Bullet AddBullet()
    {
        Bullet _RandomBullet = new Bullet(_GameWindow, _Player);
        return _RandomBullet;
    }

    private void CheckCollisions()
    {
        foreach (Robot robot in _Robots)
        {
            if (_Player.CollidedWith(robot) && _Player.Lives > 0)
            {
                _Player.Lives = _Player.Lives - 1;
            }
            if (_Player.CollidedWith(robot) || robot.IsOffscreen(_GameWindow))
            {
                _removedRobots.Add(robot);
            }
            foreach (Bullet bullet in _Bullets)
            {
                if (bullet.BulletCollidedWith(robot))
                {
                    _removedBullets.Add(bullet);
                    _removedRobots.Add(robot);
                }
                if (bullet.IsOffscreen(_GameWindow))
                {
                    _removedBullets.Add(bullet);
                }
            }
        }
        foreach (Robot robot in _removedRobots)
        {
            _Robots.Remove(robot);
        }
        foreach (Bullet bullet in _removedBullets)
        {
            _Bullets.Remove(bullet);
        }
    }

    public void DrawHUD()
    {
        DrawHearts(_Player.Lives);
        SplashKit.DrawText("SCORE: " + _Player.Score, Color.Black, 0, 40);
    }

    public void DrawHearts(int numberOfHearts)
    {
        int heartX = 0;
        for (int i = 0; i < numberOfHearts; i++)
        {
            if (heartX < 300)
            {
                SplashKit.DrawBitmap(HeartBitmap, heartX, 0);
                heartX = heartX + 40;
            }
        }
    }
}
