﻿using FarseerPhysics.Dynamics;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class MoveClass
    {
        static float turning_value = 0.020f;
        static float increasing_value = 0.025f;

        public static void controlGround(Player player, float T)
        {
            if (player.btn_jump && player.can_jump)
            {
                player.playSound("jump");

                player.can_jump = false;
                player.my_body.LinearVelocity = new Vector2(player.my_body.LinearVelocity.X, player.Jump_speed);
                return;
            }
            if (!player.btn_move_right && !player.btn_move_left)
            {
                player.my_body.LinearVelocity = new Vector2(0, 0);
                return;
            }
            else
                if(player.btn_move_right && !player.btn_move_left)
                {
                    player.my_body.LinearVelocity = new Vector2(player.Move_speed, 0);
                }
                else
                    if (!player.btn_move_right && player.btn_move_left)
                    {
                        player.my_body.LinearVelocity = new Vector2(-player.Move_speed, 0);
                    }
        }


        private static int get_my_current_direction(Player player)
        {
            if(player.my_body.LinearVelocity.X > 0)
            {
                return 1;
            }
            else
                if (player.my_body.LinearVelocity.X < 0)
                {
                    return -1;
                }
            return 0;
        }

        private static int get_my_wanted_direction(Player player)
        {
            if(player.btn_move_right)
            {
                return 1;
            }
            else
                if(player.btn_move_left)
                {
                    return -1;
                }
            return 0;
        }

        public static void controlAir(Player player, float T)
        {
            //if the player doesn't want to move right or left
            if (!player.btn_move_right && !player.btn_move_left)
            {
                //slow the player while he is in air and he doesn't have any direction
                if (get_my_current_direction(player) > 0)
                {
                    player.my_body.LinearVelocity = new Vector2(player.my_body.LinearVelocity.X - increasing_value * 4, player.my_body.LinearVelocity.Y);
                }
                else
                    if (get_my_current_direction(player) < 0)
                    {
                        player.my_body.LinearVelocity = new Vector2(player.my_body.LinearVelocity.X + increasing_value * 4, player.my_body.LinearVelocity.Y);
                    }
                return;
            }
            else
                //if the player wants to go the right
                if (player.btn_move_right && !player.btn_move_left)
                {
                    //if the player hasn't reach the full speed, we increase it every time until he does
                    if (player.my_body.LinearVelocity.X < player.Move_speed)
                    {
                        //if the player was moving to the left and now he turned right, I slowly decrease the speed, and after that increase it to the right
                        if (get_my_current_direction(player) != 0 && get_my_wanted_direction(player) != 0 && get_my_wanted_direction(player) != get_my_current_direction(player))
                        {
                            player.my_body.ApplyLinearImpulse(new Vector2(player.Move_speed * turning_value, 0));
                        }
                        else
                        {
                            player.my_body.ApplyLinearImpulse(new Vector2(player.Move_speed * increasing_value, 0));
                        }
                    }
                    else
                    {
                        player.my_body.LinearVelocity = new Vector2(player.Move_speed, player.my_body.LinearVelocity.Y);
                    }
                }
                else
                    //if the player wants to go the left
                    if (!player.btn_move_right && player.btn_move_left)
                    {
                        //if the player hasn't reach the full speed, we increase it every time until he does
                        if (player.my_body.LinearVelocity.X > -player.Move_speed)
                        {
                            //if the player was moving to the right and now he turned left, I slowly decrease the speed, and after that increase it to the left
                            if (get_my_current_direction(player) != 0 && get_my_wanted_direction(player) != 0 && get_my_wanted_direction(player) != get_my_current_direction(player))
                            {
                                player.my_body.ApplyLinearImpulse(new Vector2(-player.Move_speed * turning_value, 0));
                            }
                            else
                            {
                                player.my_body.ApplyLinearImpulse(new Vector2(-player.Move_speed * increasing_value, 0));
                            }
                        }
                        else
                        {
                            player.my_body.LinearVelocity = new Vector2(-player.Move_speed, player.my_body.LinearVelocity.Y);
                        }
                    }
        }


        public static void move_player_and_camera(Player[] player, Camera camera, Viewport vp)
        {
            bool make_bigger = false, make_smaller = true;
            for (int i = 0; i < player.Length; i++)
            {
                if ((player[i].my_body.Position.X > 0 && player[i].my_body.Position.X + 5 > vp.Width / (camera.Scale * Conversions._scale * 2)) ||
                    (player[i].my_body.Position.X < 0 && player[i].my_body.Position.X - 5 < -vp.Width / (camera.Scale * Conversions._scale * 2)))
                {
                    make_bigger = true;
                }
                else
                    if (camera.Scale < 1.0f && ((player[i].my_body.Position.X > 0 && player[i].my_body.Position.X + 6 < vp.Width / (camera.Scale * Conversions._scale * 2)) ||
                                            (player[i].my_body.Position.X < 0 && player[i].my_body.Position.X - 6 > -vp.Width / (camera.Scale * Conversions._scale * 2))))
                    {
                    }
                    else
                    {
                        make_smaller = false;
                    }
            }
            if(make_bigger)
            {
                camera.Scale -= 0.01f;
            }
            else
                if(make_smaller)
                {
                    camera.Scale += 0.01f;
                }
             
        }
    }
}
