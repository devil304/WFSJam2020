// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/inputy/KlikKlik.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @KlikKlik : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @KlikKlik()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KlikKlik"",
    ""maps"": [
        {
            ""name"": ""main"",
            ""id"": ""811f5cc0-e3b4-4b17-8a26-77418a5c75d0"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""type"": ""Value"",
                    ""id"": ""f2b4321d-a815-4c7c-ab20-dd538247d8b3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JumpSlide"",
                    ""type"": ""Button"",
                    ""id"": ""a63be762-eba2-450f-8afc-2b5b4bb6aba4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""dbfb433e-481e-4dcc-83b9-cd0ecb2ce02a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hook"",
                    ""type"": ""Button"",
                    ""id"": ""3103f6a7-bc85-4422-984a-3b778eb641fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bcdd00bf-9273-4a5b-993e-ba60e039400f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ee39001f-2f92-49ea-9bb5-1c57fcd60390"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8e415131-dd85-4e3f-8921-30b5fc64dffb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3459cecf-db73-4329-8d53-6bb0b5f184f4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c6e5262e-0020-4c8e-aa71-d995f9a7f015"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""96dca5ed-50ff-4e53-9a81-ebb62146996e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpSlide"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""731dd984-61d8-4132-9002-c3720124e28f"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpSlide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""65931211-d1d8-493e-82dd-2e2c21bd06e6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpSlide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c51d582b-fec3-4b9f-82e0-f6f5733c00c5"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0f8fc11-bfa1-45c8-bd15-d02addd6ec3c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // main
        m_main = asset.FindActionMap("main", throwIfNotFound: true);
        m_main_move = m_main.FindAction("move", throwIfNotFound: true);
        m_main_JumpSlide = m_main.FindAction("JumpSlide", throwIfNotFound: true);
        m_main_Camera = m_main.FindAction("Camera", throwIfNotFound: true);
        m_main_Hook = m_main.FindAction("Hook", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // main
    private readonly InputActionMap m_main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_main_move;
    private readonly InputAction m_main_JumpSlide;
    private readonly InputAction m_main_Camera;
    private readonly InputAction m_main_Hook;
    public struct MainActions
    {
        private @KlikKlik m_Wrapper;
        public MainActions(@KlikKlik wrapper) { m_Wrapper = wrapper; }
        public InputAction @move => m_Wrapper.m_main_move;
        public InputAction @JumpSlide => m_Wrapper.m_main_JumpSlide;
        public InputAction @Camera => m_Wrapper.m_main_Camera;
        public InputAction @Hook => m_Wrapper.m_main_Hook;
        public InputActionMap Get() { return m_Wrapper.m_main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @move.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @move.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @move.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @JumpSlide.started -= m_Wrapper.m_MainActionsCallbackInterface.OnJumpSlide;
                @JumpSlide.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnJumpSlide;
                @JumpSlide.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnJumpSlide;
                @Camera.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCamera;
                @Hook.started -= m_Wrapper.m_MainActionsCallbackInterface.OnHook;
                @Hook.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnHook;
                @Hook.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnHook;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @move.started += instance.OnMove;
                @move.performed += instance.OnMove;
                @move.canceled += instance.OnMove;
                @JumpSlide.started += instance.OnJumpSlide;
                @JumpSlide.performed += instance.OnJumpSlide;
                @JumpSlide.canceled += instance.OnJumpSlide;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Hook.started += instance.OnHook;
                @Hook.performed += instance.OnHook;
                @Hook.canceled += instance.OnHook;
            }
        }
    }
    public MainActions @main => new MainActions(this);
    public interface IMainActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJumpSlide(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnHook(InputAction.CallbackContext context);
    }
}
