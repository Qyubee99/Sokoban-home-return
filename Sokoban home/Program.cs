namespace Sokoban_home
{
	enum Direction // 방향을 저장하는 타입
	{
		None,
		Left,
		Right,
		Up,
		Down
	}
	internal class Program
	{
		static void Main()
		{
			Console.ResetColor(); // 컬러 초기화
			Console.CursorVisible = false; // 커서 숨기기
			Console.Title = "홍성재의 니코니코니";
			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Clear();

			// 기호 상수 정의
			const int GOAL_COUNT = 2;
			const int BOX_COUNT = GOAL_COUNT;

			// 플레이어 위치를 저장하기 위한 변수
			int playerX = 0;
			int playerY = 0;

			// 플레이어의 이동 방향을 저장하기 위한 변수
			Direction playerMoveDirection = Direction.None;

			// 플레이어가 무슨 박스를 밀고 있는지 저장하기 위한 변수
			int pushedBoxId = 0; // 1이면 박스1 2면 박스2

			// 박스의 위치를 저장하기 위한 변수
			int[] boxPositionsX = { 5, 15 };
			int[] boxPositionsY = { 5, 3 };

			// 벽의 위치를 저장하기 위한 변수
			int wallX = 16;
			int wallY = 8;

			// 골의 위치를 저장하기 위한 변수
			int[] goalPositionsX = { 27, 2 };
			int[] goalPositionsY = { 12, 12 };

			while (true)
			{
				// 이전 프레임을 지운다.
				Console.Clear();

				// 플레이어를 그린다.
				Console.SetCursorPosition(playerX, playerY);
				Console.Write("P");

				// 박스를 그린다
				for (int i = 0; i < BOX_COUNT; i++)
				{
					int boxX = boxPositionsX[i];
					int boxY = boxPositionsY[i];
					Console.SetCursorPosition(boxX, boxY);
					Console.Write("B");
				}

				// 벽을 그린다.
				Console.SetCursorPosition(wallX, wallY);
				Console.Write("W");

				// 골을 그린다
				for (int i = 0; i < GOAL_COUNT; i++)
				{
					int goalX = goalPositionsX[i];
					int goalY = goalPositionsY[i];
					Console.SetCursorPosition(goalX, goalY);
					Console.Write("G");
				}

				ConsoleKey key = Console.ReadKey().Key;

				// 플레이어의 이동
				if (key == ConsoleKey.LeftArrow)
				{
					playerX = Math.Max(0, playerX - 1);
					playerMoveDirection = Direction.Left;
				}
				if (key == ConsoleKey.RightArrow)
				{
					playerX = Math.Min(playerX + 1, 30);
					playerMoveDirection = Direction.Right;
				}
				if (key == ConsoleKey.UpArrow)
				{
					playerY = Math.Max(0, playerY - 1);
					playerMoveDirection = Direction.Up;
				}
				if (key == ConsoleKey.DownArrow)
				{
					playerY = Math.Min(playerY + 1, 15);
					playerMoveDirection = Direction.Down;
				}

				// 박스 밀기
				for (int i = 0; i < BOX_COUNT; i++)
				{
					int boxX = boxPositionsX[i];
					int boxY = boxPositionsY[i];

					if (playerX == boxX && playerY == boxY)
					{
						switch (playerMoveDirection)
						{
							case Direction.Left:
								boxX = Math.Max(0, boxX - 1);
								playerX = boxX + 1;
								break;
							case Direction.Right:
								boxX = Math.Min(boxX + 1, 30);
								playerX = boxX - 1;
								break;
							case Direction.Up:
								boxY = Math.Max(0, boxY - 1);
								playerY = boxY + 1;
								break;
							case Direction.Down:
								boxY = Math.Min(boxY + 1, 15);
								playerY = boxY - 1;
								break;
							default:
								Console.Clear();
								Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

								return;
						}
						pushedBoxId = i;
					}
						boxPositionsX[i] = boxX;
						boxPositionsY[i] = boxY;
				}

				// 플레이어와 벽의 충돌
				if (playerX == wallX && playerY == wallY)
				{
					switch (playerMoveDirection)
					{
						case Direction.Left:
							playerX = wallX + 1;
							break;
						case Direction.Right:
							playerX = wallX - 1;
							break;
						case Direction.Up:
							playerY = wallY + 1;
							break;
						case Direction.Down:
							playerY = wallY - 1;
							break;
						default:
							Console.Clear();
							Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

							return;

					}
				}
				// 박스와 벽의 충돌
				for (int i = 0; i < BOX_COUNT; i++)
				{
					int boxX = boxPositionsX[i];
					int boxY = boxPositionsY[i];

					if (boxX == wallX && boxY == wallY)
					{
						switch (playerMoveDirection)
						{
							case Direction.Left:
								boxX = wallX + 1;
								playerX = boxX + 1;
								break;
							case Direction.Right:
								boxX = wallX - 1;
								playerX = boxX - 1;
								break;
							case Direction.Up:
								boxY = wallY + 1;
								playerY = boxY + 1;
								break;
							case Direction.Down:
								boxY = wallY - 1;
								playerY = boxY - 1;
								break;
							default:
								Console.Clear();
								Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

								return;
						}

						break;
					}
				}

				// 박스끼리 충돌 처리
				for (int collidedBoxId = 0; collidedBoxId < BOX_COUNT; collidedBoxId++)
				{
					// 같은 박스라면 처리할 필요x
					if (pushedBoxId == collidedBoxId)
					{
						continue;
					}
					// 두 개의 박스가 부딪혔을 때
					if (boxPositionsX[pushedBoxId] == boxPositionsX[collidedBoxId] && boxPositionsY[pushedBoxId] == boxPositionsY[collidedBoxId])
					{
						switch (playerMoveDirection)
						{
							case Direction.Left:
								boxPositionsX[pushedBoxId] = boxPositionsX[collidedBoxId] + 1;
								playerX = boxPositionsX[pushedBoxId] + 1;
								break;
							case Direction.Right:
								boxPositionsX[pushedBoxId] = boxPositionsX[collidedBoxId] - 1;
								playerX = boxPositionsX[pushedBoxId] - 1;
								break;
							case Direction.Up:
								boxPositionsY[pushedBoxId] = boxPositionsY[collidedBoxId] + 1;
								playerY = boxPositionsY[pushedBoxId] + 1;
								break;
							case Direction.Down:
								boxPositionsY[pushedBoxId] = boxPositionsY[collidedBoxId] - 1;
								playerY = boxPositionsY[pushedBoxId] - 1;
								break;
							default:
								Console.Clear();
								Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

								return;
						}

						break;
					}
				}
				//	int count = 0;

				//	if ((boxPositionsX == goalX && boxY == goalY) || (boxX2 == goalX && boxY2 == goalY))
				//	{
				//		count++;
				//	}

				//	if ((boxPositionsX == goalX2 && boxY == goalY2) || (boxX2 == goalX2 && boxY2 == goalY2))
				//	{
				//		count++;
				//	}

				//	// 박스와 골의 충돌
				//	if (count == 2)
				//	{
				//		Console.Clear();
				//		Console.WriteLine("축하합니다. 클리어 하셨습니다.");
				//		break;
				//	}
				//}
				// 1월 5일 31 : 50
			}
		}
	}
}