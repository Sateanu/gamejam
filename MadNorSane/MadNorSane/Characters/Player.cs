﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Krypton;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
   public abstract class Player : Physics_object
    {

       public Stats stat;
       public bool btn_jump = false, btn_move_left = false, btn_move_right = false, btn_atack1 = false, btn_atack2 = false;
       public bool can_jump = true, can_move_left = false, can_move_right = false, can_atack1 = false, can_atack2 = false;
       public Texture2D heart;
       public Texture2D arrowtext;
       public Texture2D heartMP;
       public Texture2D tinta;
       public Texture2D health_color;
       public float tintaAngle;
       public List<Modifier> modifiers;
       public int score = 0;
       public Color color;
       public SoundManager soundManager;
       public ShadowHull hull;
       Random r = new Random((int)DateTime.Now.Ticks);
       public void setAngle(float f)
       {
           tintaAngle = f;
       }
       public void DrawTinta(SpriteBatch spriteBatch)
       {
           float radius = width > height ? width : height;
           
           spriteBatch.Draw(tinta, new Rectangle(
               (int)Conversions.to_pixels(my_body.Position.X+(float)Math.Sin(tintaAngle)),
               (int)Conversions.to_pixels(my_body.Position.Y-(float)Math.Cos(tintaAngle)),
               (int)Conversions.to_pixels(radius),
               (int)Conversions.to_pixels(radius)), null, color, tintaAngle, new Vector2(tinta.Width / 2, tinta.Height / 2), SpriteEffects.None, 0f);
       }
       public float MP
        {
            get { return stat.mana_points; }
            set { stat.mana_points = value; }
        }

        public float HP
        {
            get { return stat.health_points; }
            set { stat.health_points = value; }
        }

        public float Jump_speed
        {
            get { return stat.jump_speed; }
            set { stat.jump_speed = value; }
        }

        public float Move_speed
        {
            get { return stat.move_speed; }
            set { stat.move_speed = value; }
        }
        bool has_weapon = true;


        public virtual void atack(Vector2 direction, int _my_skill, GameTime _game_time,KryptonEngine krypthon, Texture2D tex)
        {
           
        }
        public bool use_buff(String _skill)
        {
            return true;
        }
        public void jump()
        {

        }
        public void move_on_ground()
        {
            MoveClass.controlGround(this, speed_float);
        }

        public void controlAir()
        {
            MoveClass.controlAir(this, 0.95f);
        }

       public void playSound(String sound)
        {
            SoundManager.playSound(sound);
        }

       public virtual void Draw(SpriteBatch spriteBatch)
        {
            //animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
        }
       int vibTTL = 35;
       public virtual void Update(GameTime gameTime)
       {
           this.hull.Position = Conversions.to_pixels(my_body.Position);
           if (vibTTL > 0)
           { vibTTL--; 
               if(vibTTL==1)
           GamePad.SetVibration(0, 0f, 0f);}
           
           
       }
       public void DrawUI(Player player, SpriteBatch spriteBatch, int cadran, Viewport viewport)
       {
           //spriteBatch.Draw(player.my_texture, new Rectangle((int)Conversions.to_pixels(player.my_body.Position.X), (int)Conversions.to_pixels(player.my_body.Position.Y), 32, 32), Color.Red);
           int i; int mx;
           int y = 0;
           int item_width = 34;
           int length = player.stat.original_health_points > player.stat.original_arrow_nr ? length = (int)player.stat.original_health_points : length = (int)player.stat.original_arrow_nr;
           length = player.stat.original_health_points < player.stat.original_mana_points ? length = (int)player.stat.original_mana_points : length = (int)player.stat.original_health_points;
            switch(cadran)
            {
                case 0:
                    
                    for (i = 0; i < player.stat.health_points; i++)
                        spriteBatch.Draw(heart, new Rectangle(i * item_width, 0, 32, 32), Color.White);
                    spriteBatch.Draw(my_texture, new Rectangle(0, (int)(72), (int)(64 * width), (int)(64 * height)), Color.White);
                    if (player.GetType() == typeof(Mage))
                        for (i = 0; i < stat.mana_points; i++)
                            spriteBatch.Draw(heartMP, new Rectangle(i * item_width, item_width, 32, 32), Color.White);
                    if (player.GetType() == typeof(Archer))
                    for (i = 0; i < stat.arrownr; i++)
                        spriteBatch.Draw(arrowtext, new Rectangle(i * item_width, item_width, 32, 32), player.color);

                    break;
                case 1:
                    for (i =0; i < player.stat.health_points; i++)
                        spriteBatch.Draw(heart, new Rectangle(viewport.Width - i * item_width - item_width, 0, 32, 32), Color.White);
                    spriteBatch.Draw(my_texture, new Rectangle(viewport.Width - (int)(64 * height), (int)(72), (int)(64 * width), (int)(64 * height)), Color.White);
                    if(player.GetType()==typeof(Mage))
                    for (i = 0; i < player.stat.mana_points; i++)
                        spriteBatch.Draw(heartMP, new Rectangle(viewport.Width - i * item_width - item_width, item_width, 32, 32), Color.White);
                    if (player.GetType() == typeof(Archer))
                    for (i = 0; i < stat.arrownr; i++)
                        spriteBatch.Draw(arrowtext, new Rectangle(viewport.Width - i * item_width - item_width, item_width, 32, 32), player.color);
                    break;
                default:
                    break;
            }
       }
        public void move_in_air()
        {
        }

        public void TakeDamage(int damage)
        {
            playSound("pain"+r.Next(1,4).ToString());
            GamePad.SetVibration(0, 1f, 1f);
            vibTTL = 35;
            this.HP -= damage;
        }
        public bool VS_OnCollision(Fixture fixA, Fixture fixB, Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {
                
                if (fixA.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                {
                    if (fixB.Body.UserData == "ground" && touched_sides.Y > 0)
                    {
                        can_jump = true;
                    }
                    else
                    if (fixB.Body.UserData == "ground" && touched_sides.Y < 0)
                    {
                        can_jump = false;
                    }
                    else
                    if (fixB.Body.UserData == "wall" && touched_sides.X > 0)
                    {
                        //can_jump = false;
                        Console.WriteLine("is on right side of the wall");
                    }
                    else
                    if (fixB.Body.UserData == "wall" && touched_sides.X < 0)
                    {
                        Console.WriteLine("is on left side of the wall");
                        //can_jump = false;
                    }
                    else
                        if (fixB.Body.UserData == "wall" && touched_sides.Y > 0)
                        {
                            Console.WriteLine("i'm on the wall");
                            can_jump = true;
                        }


                    if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))// && touched_sides.X != 0)
                    {
                        Console.WriteLine("Am lovit player");
                        return false;
                    }

                }

            }
            return true;
        }

    }
}
