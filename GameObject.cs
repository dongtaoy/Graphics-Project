using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public enum GameObjectType
    {
        NONDRAWABLE, DRAWABLE
    }

    // Super class for all game objects.
    abstract public class GameObject
    {
        //public ProjectGame game;
        protected GameObjectType gameObjectType;
        protected ProjectGame game;
        // public Vector3 pos;
        // public BasicEffect basicEffect;

        // Properties
        public GameObjectType GameObjectType { get { return gameObjectType; } set { gameObjectType = value; } }
        public ProjectGame ProjectGame { get { return game; } set { game = value; } }
        
        public GameObject()
        {

        }

        public GameObject(ProjectGame game)
        {
            this.gameObjectType = GameObjectType.NONDRAWABLE;
            this.game = game;
            this.game.Add(this);
        }


        public abstract void Update(GameTime gametime);
        

        public void GetParamsFromModel()
        {
            //if (myModel.modelType == ModelType.Colored) {
            //    basicEffect = new BasicEffect(game.GraphicsDevice)
            //    {
            //        View = game.camera.View,
            //        Projection = game.camera.Projection,
            //        World = Matrix.Identity,
            //        VertexColorEnabled = true
            //    };
            //}
            //else if (myModel.modelType == ModelType.Textured) {
            //    basicEffect = new BasicEffect(game.GraphicsDevice)
            //    {
            //        View = game.camera.View,
            //        Projection = game.camera.Projection,
            //        World = Matrix.Identity,
            //        Texture = myModel.Texture,
            //        TextureEnabled = true,
            //        VertexColorEnabled = false
            //    };
            //}
        }

        // These virtual voids allow any object that extends GameObject to respond to tapped and manipulation events
        public virtual void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {

        }

        public virtual void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {

        }

        public virtual void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {

        }

        public virtual void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }
    }
}
