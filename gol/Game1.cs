using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{

	public class LifeOfGame : Game
	{
		GraphicsDeviceManager Graphics;
		SpriteBatch SpriteBatch;
		//细胞
		Texture2D AutomatonImg;
		//鼠标
		Texture2D CursorImg;
		KeyboardState KeyBoardState;
		MouseState mouseState;

		//地图
		int[,,] Map1;
		//地图宽高
		int W, H;
		//刷新率
		int Fps;
		//时间
		int Time;
		//细胞受影响的范围(半径)
		int Range;

		bool IsRun;
		//按键状态
		bool KeyState;



		public LifeOfGame()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			KeyBoardState = Keyboard.GetState();

			W = Graphics.PreferredBackBufferWidth / 3;
			H = Graphics.PreferredBackBufferHeight / 3;

			Fps = 5;
			Time = 0;
			Range = 1;

			IsRun = false;
			KeyState = true;
		}


		protected override void Initialize()
		{
			Map1 = new int[W, H, 2];

			#region 初始化地图
			if (W > 150 && H > 120)
				for (int i = 0; i < 41; ++i)
				{
					Map1[100 + i, 80, 0] = 1;
				}
			#endregion

			base.Initialize();
		}


		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			AutomatonImg = Content.Load<Texture2D>("Automaton");
			CursorImg = Content.Load<Texture2D>("Cursor");
		}


		protected override void Update(GameTime gameTime)
		{
			++Time;
			mouseState = Mouse.GetState();
			//防止某BUG再次出现
			if (Keyboard.GetState() != KeyBoardState)
			{
				KeyBoardState = Keyboard.GetState();
				KeyState = true;
			}
			else
			{
				KeyState = false;
			}
			//运行/停止
			if (KeyBoardState.IsKeyDown(Keys.Space) && KeyState)
			{
				IsRun = !IsRun;
			}
			//画细胞
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				int x_ = Mouse.GetState().X / 3;
				int y_ = Mouse.GetState().Y / 3;
				if (x_ >= 0 && x_ < W && y_ >= 0 && y_ < H)
				{
					Map1[x_, y_, 0] = 1;
				}
			}
			//打死细胞
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				int x_ = Mouse.GetState().X / 3;
				int y_ = Mouse.GetState().Y / 3;
				if (x_ >= 0 && x_ < W && y_ >= 0 && y_ < H)
				{
					Map1[x_, y_, 0] = 0;
				}
			}

			//设置Fps
			if (KeyBoardState.IsKeyDown(Keys.S) && KeyState && Fps < 1024)
			{
				++Fps;
			}
			if (KeyBoardState.IsKeyDown(Keys.W) && KeyState && Fps > 1)
			{
				--Fps;
			}
			if ((IsRun && Time >= Fps) || (KeyBoardState.IsKeyDown(Keys.A) && KeyState))
			{
				Time = 0;

				for (int i = 0; i < H; ++i)
				{
					for (int j = 0; j < W; ++j)
					{
						if (Map1[j, i, 0] == 1)
						{
							for (int i1 = i - Range; i1 <= i + Range; ++i1)
							{
								for (int j1 = j - Range; j1 <= j + Range; ++j1)
								{
									if (i1 >= 0 && j1 >= 0 && i1 < H && j1 < W && (j1 != j || i1 != i))
									{
										++Map1[j1, i1, 1];
									}
								}
							}
						}
					}
				}
				for (int i = 0; i < H; ++i)
				{
					for (int j = 0; j < W; ++j)
					{
						if (Map1[j, i, 1] > 3 || Map1[j, i, 1] < 2)
						{
							Map1[j, i, 0] = 0;
						}
						if (Map1[j, i, 1] == 3)
						{
							Map1[j, i, 0] = 1;
						}
						Map1[j, i, 1] = 0;
					}
				}
			}
			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			//开始画了
			SpriteBatch.Begin();
			for (int i = 0; i < H; ++i)
			{
				for (int j = 0; j < W; ++j)
				{
					if (Map1[j, i, 0] == 1)
					{
						SpriteBatch.Draw(AutomatonImg, new Vector2(j * 3, i * 3), Color.White);
					}
				}
			}
			//光标
			SpriteBatch.Draw(CursorImg, new Vector2(mouseState.X / 3 * 3, mouseState.Y / 3 * 3), Color.White);
			//画完了
			SpriteBatch.End();


			base.Draw(gameTime);
		}
	}
}
