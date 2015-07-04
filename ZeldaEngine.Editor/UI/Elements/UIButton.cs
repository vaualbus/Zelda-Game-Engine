using SharpDX;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Game.Extensions;
using ZeldaEngine.SharpDxImp.DataFormat;

namespace ZeldaEngine.Game.UI.Elements
{
    public class UIButton : UIElement
    {
        private bool _isMouseHover;
        private bool _isFirstPressed;
        private bool _isHiglighting;

        private Texture2D _btnTexture;

        public Color PushedColor { get; set; }

        public Color BorderColor { get; set; }

        public int BorderWidth { get; set; }

        public IResourceData Texture { get; set; }

        public UIButton(IGameEngine gameEngine) 
            : base(gameEngine)
        {
            _isMouseHover = false;
            _isHiglighting = false;
        }

        public override void OnInit()
        {
            _btnTexture = Texture != null ? (Texture2D) Texture.Value : (Texture2D) GameEngine.RenderEngine.GenerateEmptyTexture2D(Width, Height).Value;
            if (Texture == null)
                _btnTexture.ApplyColor(Color);
        }

        public override void OnDraw(IRenderEngine renderEngine)
        {
            //if (_isHiglighting)
            //{
            //    renderEngine.DrawBorder(new Texture2DResourceData(_btnTexture), BorderWidth, BorderColor);
            //}

            renderEngine.DrawTexture(Position, new Texture2DResourceData(_btnTexture), Width, Height, _isMouseHover ? PushedColor : Color);
        }

        public override void OnUpdate(float dt)
        {
            var mousePosX = GameEngine.InputManager.MousePosition.X;
            var mousePosY = GameEngine.InputManager.MousePosition.Y;

            //If the mouse inside, highlight the box
            if ((mousePosX >= Position.X && mousePosX <= Position.X + Width) &&
               (mousePosY >= Position.Y && mousePosY <= Position.Y + Height))
                _isHiglighting = true;

            if ((mousePosX >= Position.X && mousePosX <= Position.X + Width) &&
                (mousePosY >= Position.Y && mousePosY <= Position.Y + Height) &&
                GameEngine.InputManager.IsMouseButtonPressed("Left") && !GameEngine.InputManager.IsMouseButtonReleased("Left"))
            {
                _isMouseHover = true;

                if (!_isFirstPressed)
                {
                    //We need to invoke the actions
                    foreach (var a in UiActions)
                        a.Invoke();

                    _isFirstPressed = true;
                }
            }
            else
            {
                _isMouseHover = false;
                _isHiglighting = false;
                _isFirstPressed = false;
            }
        }
    }
}