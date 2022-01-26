using System;

namespace Battleship
{
    public class SuperCoolAgent : BattleshipAgent
    {
        Random rng = new Random();

        char[,] attackHistory;
        char searchMode = 'H'; // (hunt/attack)
        char attackDirection;
        GridSquare attackGrid;
        GridSquare savedCoordinates;

        public SuperCoolAgent()
        {
            attackHistory = new char[10, 10];
            for (int i = 0; i < attackHistory.GetLength(0); i++)
            {
                for (int j = 0; j < attackHistory.GetLength(1); j++)
                {
                    attackHistory[i, j] = 'U';
                }
            }
            attackGrid = new GridSquare();

            attackGrid.x = 6;
            attackGrid.y = 5;
        }

        public override void Initialize()
        {
            searchMode = 'H'; // (hunt/attack)
            attackDirection = 'N';
            savedCoordinates.x = -1;
            savedCoordinates.y = -1;
            return;
        }

        public override string ToString()
        {

            return $"Battleship Agent '{GetNickname()}'";
        }

        public override string GetNickname()
        {

            return "Bot";
        }

        public override void SetOpponent(string opponent)
        {
            return;
        }

        public override GridSquare LaunchAttack
        {
            get
            {

                if (attackDirection == 'N' && attackGrid.y - 1 >= 0)
                {
                    if (attackGrid.y - 1 >= 0)
                    {
                        attackGrid.y -= 1;
                    }
                    else
                    {
                        attackDirection = 'E';
                        attackGrid.x += 1;
                    }

                }
                else if (attackDirection == 'E' && attackGrid.x + 1 <= 9)
                {
                    attackGrid.x += 1;
                }
                else if (attackDirection == 'S' && attackGrid.y - 1 >= 0)
                {
                    attackGrid.y += 1;
                }
                else if (attackDirection == 'W' && attackGrid.x + 1 <= 9)
                {
                    attackGrid.x -= 1;
                }


                if (attackGrid.x < 0 || attackGrid.x > 9 || attackGrid.y < 0 || attackGrid.y > 9)
                {
                    do
                    {
                        attackGrid.x = rng.Next() % 10;
                        attackGrid.y = rng.Next() % 10;
                    } while (attackHistory[attackGrid.x, attackGrid.y] != 'U');
                }

                return attackGrid;
            }
        }

        /*while (attackHistory[attackGrid.x, attackGrid.y] != 'U')
        {
            attackGrid.x = rng.Next(10);
            attackGrid.y = rng.Next(10);
        }


        if(attackGrid.y < 9)
        {
            attackGrid.y += 1;
        }
        else
        {
            attackGrid.y = 0;
            if (attackGrid.x < 9)
            {
                attackGrid.x += 1;
            }
            else
            {
                attackGrid.x = 0;
            }

        }*/



        /* if it hits, next shot is one up
         * if miss, down one, right one
         * if miss, left one, down one,
         * if miss, up one, left one
         * ^if any hit, continue until a miss
         */


        public override void DamageReport(char report)
        {
            if (report != '\0')
            {
                searchMode = 'A';
                savedCoordinates.x = attackGrid.x;
                savedCoordinates.y = attackGrid.y;

            }
            if (searchMode == 'A')
            {
                if (attackDirection == 'N' && report == '\0')
                {
                    {
                        attackDirection = 'E';
                        attackGrid.x = savedCoordinates.x;
                        attackGrid.y = savedCoordinates.y;
                    }
                }
                else if (attackDirection == 'E' && report == '\0')
                {
                    {
                        attackDirection = 'S';
                        attackGrid.x = savedCoordinates.x;
                        attackGrid.y = savedCoordinates.y;
                    }
                }
                else if (attackDirection == 'S' && report == '\0')
                {
                    {
                        attackDirection = 'W';
                        attackGrid.x = savedCoordinates.x;
                        attackGrid.y = savedCoordinates.y;
                    }
                }

                else if (attackDirection == 'W' && report == '\0')
                {
                    searchMode = 'H';
                    attackGrid.x = savedCoordinates.x;
                    attackGrid.y = savedCoordinates.y;
                }
            }
            //report records chosen coordinates (C,B,P,S,D,\0 or ShipType.-)
            if (report == '\0')
            {
                attackHistory[attackGrid.x, attackGrid.y] = 'M';
            }
            else
            {
                attackHistory[attackGrid.x, attackGrid.y] = report;
            }

        }

        public override BattleshipFleet PositionFleet()
        {
            BattleshipFleet myFleet = new BattleshipFleet();
            //ship position of bot
            myFleet.Carrier = new ShipPosition(3, 9, ShipRotation.Horizontal);
            myFleet.Battleship = new ShipPosition(9, 4, ShipRotation.Vertical);
            myFleet.Destroyer = new ShipPosition(2, 0, ShipRotation.Vertical);
            myFleet.Submarine = new ShipPosition(3, 0, ShipRotation.Vertical);
            myFleet.PatrolBoat = new ShipPosition(4, 0, ShipRotation.Horizontal);

            Random rng = new Random();

            if (rng.Next() % 2 == 0)
            {
                myFleet.Submarine = new ShipPosition(5, 5, ShipRotation.Vertical);
            }
            else
            {
                myFleet.Submarine = new ShipPosition(5, 5, ShipRotation.Horizontal);
            }


            return myFleet;
        }
    }
}

