﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class Warrior : Physics_object
    {
        Warrior(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;

            set_texture("warrior");
        }
    }
}
