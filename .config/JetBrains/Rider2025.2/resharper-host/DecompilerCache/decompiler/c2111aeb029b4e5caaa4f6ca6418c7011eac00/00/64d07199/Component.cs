// Decompiled with JetBrains decompiler
// Type: UnityEngine.Component
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2111AEB-029B-4E5C-AAA4-F6CA6418C701
// Assembly location: /home/jdw/Unity/Hub/Editor/6000.1.13f1/Editor/Data/Managed/UnityEngine/UnityEngine.CoreModule.dll
// XML documentation location: /home/jdw/Unity/Hub/Editor/6000.1.13f1/Editor/Data/Managed/UnityEngine/UnityEngine.CoreModule.xml

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

#nullable disable
namespace UnityEngine;

/// <summary>
///   <para>Base class for everything attached to a GameObject.</para>
/// </summary>
[RequiredByNativeCode]
[NativeClass("Unity::Component")]
[NativeHeader("Runtime/Export/Scripting/Component.bindings.h")]
public class Component : Object
{
  /// <summary>
  ///   <para>The Transform attached to this GameObject.</para>
  /// </summary>
  public Transform transform
  {
    [FreeFunction("GetTransform", HasExplicitThis = true, ThrowsException = true)] get
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      return Unmarshal.UnmarshalUnityObject<Transform>(Component.get_transform_Injected(_unity_self));
    }
  }

  /// <summary>
  ///   <para>The game object this component is attached to. A component is always attached to a game object.</para>
  /// </summary>
  public GameObject gameObject
  {
    [FreeFunction("GetGameObject", HasExplicitThis = true)] get
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      return Unmarshal.UnmarshalUnityObject<GameObject>(Component.get_gameObject_Injected(_unity_self));
    }
  }

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="type">The type of Component to retrieve.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public Component GetComponent(System.Type type) => this.gameObject.GetComponent(type);

  [FreeFunction(HasExplicitThis = true, ThrowsException = true)]
  internal void GetComponentFastPath(System.Type type, IntPtr oneFurtherThanResultValue)
  {
    IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
    if (_unity_self == IntPtr.Zero)
      ThrowHelper.ThrowNullReferenceException((object) this);
    Component.GetComponentFastPath_Injected(_unity_self, type, oneFurtherThanResultValue);
  }

  [SecuritySafeCritical]
  public unsafe T GetComponent<T>()
  {
    CastHelper<T> castHelper = new CastHelper<T>();
    this.GetComponentFastPath(typeof (T), new IntPtr((void*) &castHelper.onePointerFurtherThanT));
    return castHelper.t;
  }

  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public bool TryGetComponent(System.Type type, out Component component)
  {
    return this.gameObject.TryGetComponent(type, out component);
  }

  [SecuritySafeCritical]
  public bool TryGetComponent<T>(out T component)
  {
    return this.gameObject.TryGetComponent<T>(out component);
  }

  /// <summary>
  ///   <para>The string-based version of this method.</para>
  /// </summary>
  /// <param name="type">The name of the type of Component to get.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [FreeFunction(HasExplicitThis = true)]
  public unsafe Component GetComponent(string type)
  {
    IntPtr componentInjected;
    Component component;
    try
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      ManagedSpanWrapper managedSpanWrapper;
      if (!StringMarshaller.TryMarshalEmptyOrNullString(type, ref managedSpanWrapper))
      {
        ReadOnlySpan<char> readOnlySpan = type.AsSpan();
        fixed (char* begin = &readOnlySpan.GetPinnableReference())
          managedSpanWrapper = new ManagedSpanWrapper((void*) begin, readOnlySpan.Length);
      }
      componentInjected = Component.GetComponent_Injected(_unity_self, ref managedSpanWrapper);
    }
    finally
    {
      component = Unmarshal.UnmarshalUnityObject<Component>(componentInjected);
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(begin);
    }
    return component;
  }

  /// <summary>
  ///   <para>This is the non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public Component GetComponentInChildren(System.Type t, bool includeInactive)
  {
    return this.gameObject.GetComponentInChildren(t, includeInactive);
  }

  /// <summary>
  ///   <para>This is the non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public Component GetComponentInChildren(System.Type t) => this.GetComponentInChildren(t, false);

  public T GetComponentInChildren<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive)
  {
    return (T) this.GetComponentInChildren(typeof (T), includeInactive);
  }

  [ExcludeFromDocs]
  public T GetComponentInChildren<T>() => (T) this.GetComponentInChildren(typeof (T), false);

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
  /// <returns>
  ///   <para>An array of all found components matching the specified type.</para>
  /// </returns>
  public Component[] GetComponentsInChildren(System.Type t, bool includeInactive)
  {
    return this.gameObject.GetComponentsInChildren(t, includeInactive);
  }

  [ExcludeFromDocs]
  public Component[] GetComponentsInChildren(System.Type t)
  {
    return this.gameObject.GetComponentsInChildren(t, false);
  }

  public T[] GetComponentsInChildren<T>(bool includeInactive)
  {
    return this.gameObject.GetComponentsInChildren<T>(includeInactive);
  }

  public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
  {
    this.gameObject.GetComponentsInChildren<T>(includeInactive, result);
  }

  public T[] GetComponentsInChildren<T>() => this.GetComponentsInChildren<T>(false);

  public void GetComponentsInChildren<T>(List<T> results)
  {
    this.GetComponentsInChildren<T>(false, results);
  }

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public Component GetComponentInParent(System.Type t, bool includeInactive)
  {
    return this.gameObject.GetComponentInParent(t, includeInactive);
  }

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
  /// <returns>
  ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
  /// </returns>
  [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
  public Component GetComponentInParent(System.Type t)
  {
    return this.gameObject.GetComponentInParent(t, false);
  }

  public T GetComponentInParent<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive)
  {
    return (T) this.GetComponentInParent(typeof (T), includeInactive);
  }

  public T GetComponentInParent<T>() => (T) this.GetComponentInParent(typeof (T), false);

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="t">The type of component to search for.</param>
  /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
  /// <returns>
  ///   <para>An array of all found components matching the specified type.</para>
  /// </returns>
  public Component[] GetComponentsInParent(System.Type t, [UnityEngine.Internal.DefaultValue("false")] bool includeInactive)
  {
    return this.gameObject.GetComponentsInParent(t, includeInactive);
  }

  [ExcludeFromDocs]
  public Component[] GetComponentsInParent(System.Type t) => this.GetComponentsInParent(t, false);

  public T[] GetComponentsInParent<T>(bool includeInactive)
  {
    return this.gameObject.GetComponentsInParent<T>(includeInactive);
  }

  public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
  {
    this.gameObject.GetComponentsInParent<T>(includeInactive, results);
  }

  public T[] GetComponentsInParent<T>() => this.GetComponentsInParent<T>(false);

  /// <summary>
  ///   <para>The non-generic version of this method.</para>
  /// </summary>
  /// <param name="type">The type of component to search for.</param>
  /// <returns>
  ///   <para>An array containing all matching components of type type.</para>
  /// </returns>
  public Component[] GetComponents(System.Type type) => this.gameObject.GetComponents(type);

  [FreeFunction(HasExplicitThis = true, ThrowsException = true)]
  private void GetComponentsForListInternal(System.Type searchType, object resultList)
  {
    IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
    if (_unity_self == IntPtr.Zero)
      ThrowHelper.ThrowNullReferenceException((object) this);
    Component.GetComponentsForListInternal_Injected(_unity_self, searchType, resultList);
  }

  public void GetComponents(System.Type type, List<Component> results)
  {
    this.GetComponentsForListInternal(type, (object) results);
  }

  public void GetComponents<T>(List<T> results)
  {
    this.GetComponentsForListInternal(typeof (T), (object) results);
  }

  /// <summary>
  ///   <para>The tag of this game object.</para>
  /// </summary>
  public string tag
  {
    get => this.gameObject.tag;
    set => this.gameObject.tag = value;
  }

  public T[] GetComponents<T>() => this.gameObject.GetComponents<T>();

  /// <summary>
  ///   <para>Gets the index of the component on its parent GameObject.</para>
  /// </summary>
  /// <returns>
  ///   <para>The component index.</para>
  /// </returns>
  public int GetComponentIndex()
  {
    IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
    if (_unity_self == IntPtr.Zero)
      ThrowHelper.ThrowNullReferenceException((object) this);
    return Component.GetComponentIndex_Injected(_unity_self);
  }

  /// <summary>
  ///   <para>Checks the GameObject's tag against the defined tag.</para>
  /// </summary>
  /// <param name="tag">The tag to compare.</param>
  /// <returns>
  ///   <para>Returns true if GameObject has same tag. Returns false otherwise.</para>
  /// </returns>
  public bool CompareTag(string tag) => this.gameObject.CompareTag(tag);

  /// <summary>
  ///   <para>Checks the GameObject's tag against the defined tag.</para>
  /// </summary>
  /// <param name="tag">A TagHandle representing the tag to compare.</param>
  /// <returns>
  ///   <para>Returns true if GameObject has same tag. Returns false otherwise.</para>
  /// </returns>
  public bool CompareTag(TagHandle tag) => this.gameObject.CompareTag(tag);

  [FreeFunction(HasExplicitThis = true)]
  internal Component GetCoupledComponent()
  {
    IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
    if (_unity_self == IntPtr.Zero)
      ThrowHelper.ThrowNullReferenceException((object) this);
    return Unmarshal.UnmarshalUnityObject<Component>(Component.GetCoupledComponent_Injected(_unity_self));
  }

  [FreeFunction(HasExplicitThis = true)]
  internal bool IsCoupledComponent()
  {
    IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
    if (_unity_self == IntPtr.Zero)
      ThrowHelper.ThrowNullReferenceException((object) this);
    return Component.IsCoupledComponent_Injected(_unity_self);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
  /// </summary>
  /// <param name="methodName">Name of method to call.</param>
  /// <param name="value">Optional parameter value for the method.</param>
  /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
  [FreeFunction(HasExplicitThis = true)]
  public unsafe void SendMessageUpwards(
    string methodName,
    [UnityEngine.Internal.DefaultValue("null")] object value,
    [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options)
  {
    try
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      ManagedSpanWrapper managedSpanWrapper;
      if (!StringMarshaller.TryMarshalEmptyOrNullString(methodName, ref managedSpanWrapper))
      {
        ReadOnlySpan<char> readOnlySpan = methodName.AsSpan();
        fixed (char* begin = &readOnlySpan.GetPinnableReference())
          managedSpanWrapper = new ManagedSpanWrapper((void*) begin, readOnlySpan.Length);
      }
      Component.SendMessageUpwards_Injected(_unity_self, ref managedSpanWrapper, value, options);
    }
    finally
    {
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(begin);
    }
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
  /// </summary>
  /// <param name="methodName">Name of method to call.</param>
  /// <param name="value">Optional parameter value for the method.</param>
  /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
  [ExcludeFromDocs]
  public void SendMessageUpwards(string methodName, object value)
  {
    this.SendMessageUpwards(methodName, value, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
  /// </summary>
  /// <param name="methodName">Name of method to call.</param>
  /// <param name="value">Optional parameter value for the method.</param>
  /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
  [ExcludeFromDocs]
  public void SendMessageUpwards(string methodName)
  {
    this.SendMessageUpwards(methodName, (object) null, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
  /// </summary>
  /// <param name="methodName">Name of method to call.</param>
  /// <param name="value">Optional parameter value for the method.</param>
  /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
  public void SendMessageUpwards(string methodName, SendMessageOptions options)
  {
    this.SendMessageUpwards(methodName, (object) null, options);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="value">Optional parameter for the method.</param>
  /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
  public void SendMessage(string methodName, object value)
  {
    this.SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="value">Optional parameter for the method.</param>
  /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
  public void SendMessage(string methodName)
  {
    this.SendMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="value">Optional parameter for the method.</param>
  /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
  [FreeFunction("SendMessage", HasExplicitThis = true)]
  public unsafe void SendMessage(string methodName, object value, SendMessageOptions options)
  {
    try
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      ManagedSpanWrapper managedSpanWrapper;
      if (!StringMarshaller.TryMarshalEmptyOrNullString(methodName, ref managedSpanWrapper))
      {
        ReadOnlySpan<char> readOnlySpan = methodName.AsSpan();
        fixed (char* begin = &readOnlySpan.GetPinnableReference())
          managedSpanWrapper = new ManagedSpanWrapper((void*) begin, readOnlySpan.Length);
      }
      Component.SendMessage_Injected(_unity_self, ref managedSpanWrapper, value, options);
    }
    finally
    {
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(begin);
    }
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="value">Optional parameter for the method.</param>
  /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
  public void SendMessage(string methodName, SendMessageOptions options)
  {
    this.SendMessage(methodName, (object) null, options);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
  /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
  [FreeFunction("BroadcastMessage", HasExplicitThis = true)]
  public unsafe void BroadcastMessage(
    string methodName,
    [UnityEngine.Internal.DefaultValue("null")] object parameter,
    [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options)
  {
    try
    {
      IntPtr _unity_self = Object.MarshalledUnityObject.MarshalNotNull<Component>(this);
      if (_unity_self == IntPtr.Zero)
        ThrowHelper.ThrowNullReferenceException((object) this);
      ManagedSpanWrapper managedSpanWrapper;
      if (!StringMarshaller.TryMarshalEmptyOrNullString(methodName, ref managedSpanWrapper))
      {
        ReadOnlySpan<char> readOnlySpan = methodName.AsSpan();
        fixed (char* begin = &readOnlySpan.GetPinnableReference())
          managedSpanWrapper = new ManagedSpanWrapper((void*) begin, readOnlySpan.Length);
      }
      Component.BroadcastMessage_Injected(_unity_self, ref managedSpanWrapper, parameter, options);
    }
    finally
    {
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(begin);
    }
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
  /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
  [ExcludeFromDocs]
  public void BroadcastMessage(string methodName, object parameter)
  {
    this.BroadcastMessage(methodName, parameter, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
  /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
  [ExcludeFromDocs]
  public void BroadcastMessage(string methodName)
  {
    this.BroadcastMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);
  }

  /// <summary>
  ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
  /// </summary>
  /// <param name="methodName">Name of the method to call.</param>
  /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
  /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
  public void BroadcastMessage(string methodName, SendMessageOptions options)
  {
    this.BroadcastMessage(methodName, (object) null, options);
  }

  /// <summary>
  ///   <para>The Rigidbody attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property rigidbody has been deprecated. Use GetComponent<Rigidbody>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component rigidbody
  {
    get => throw new NotSupportedException("rigidbody property has been deprecated");
  }

  /// <summary>
  ///   <para>The Rigidbody2D that is attached to the Component's GameObject.</para>
  /// </summary>
  [Obsolete("Property rigidbody2D has been deprecated. Use GetComponent<Rigidbody2D>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component rigidbody2D
  {
    get => throw new NotSupportedException("rigidbody2D property has been deprecated");
  }

  /// <summary>
  ///   <para>The Camera attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property camera has been deprecated. Use GetComponent<Camera>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component camera => throw new NotSupportedException("camera property has been deprecated");

  /// <summary>
  ///   <para>The Light attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property light has been deprecated. Use GetComponent<Light>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component light => throw new NotSupportedException("light property has been deprecated");

  /// <summary>
  ///   <para>The Animation attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property animation has been deprecated. Use GetComponent<Animation>() instead. (UnityUpgradable)", true)]
  public Component animation
  {
    get => throw new NotSupportedException("animation property has been deprecated");
  }

  /// <summary>
  ///   <para>The ConstantForce attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property constantForce has been deprecated. Use GetComponent<ConstantForce>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component constantForce
  {
    get => throw new NotSupportedException("constantForce property has been deprecated");
  }

  /// <summary>
  ///   <para>The Renderer attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property renderer has been deprecated. Use GetComponent<Renderer>() instead. (UnityUpgradable)", true)]
  public Component renderer
  {
    get => throw new NotSupportedException("renderer property has been deprecated");
  }

  /// <summary>
  ///   <para>The AudioSource attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property audio has been deprecated. Use GetComponent<AudioSource>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component audio => throw new NotSupportedException("audio property has been deprecated");

  /// <summary>
  ///   <para>The NetworkView attached to this GameObject (Read Only). (null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property networkView has been deprecated. Use GetComponent<NetworkView>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component networkView
  {
    get => throw new NotSupportedException("networkView property has been deprecated");
  }

  /// <summary>
  ///   <para>The Collider attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property collider has been deprecated. Use GetComponent<Collider>() instead. (UnityUpgradable)", true)]
  public Component collider
  {
    get => throw new NotSupportedException("collider property has been deprecated");
  }

  /// <summary>
  ///   <para>The Collider2D component attached to the object.</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property collider2D has been deprecated. Use GetComponent<Collider2D>() instead. (UnityUpgradable)", true)]
  public Component collider2D
  {
    get => throw new NotSupportedException("collider2D property has been deprecated");
  }

  /// <summary>
  ///   <para>The HingeJoint attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [Obsolete("Property hingeJoint has been deprecated. Use GetComponent<HingeJoint>() instead. (UnityUpgradable)", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Component hingeJoint
  {
    get => throw new NotSupportedException("hingeJoint property has been deprecated");
  }

  /// <summary>
  ///   <para>The ParticleSystem attached to this GameObject. (Null if there is none attached).</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Property particleSystem has been deprecated. Use GetComponent<ParticleSystem>() instead. (UnityUpgradable)", true)]
  public Component particleSystem
  {
    get => throw new NotSupportedException("particleSystem property has been deprecated");
  }

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr get_transform_Injected(IntPtr _unity_self);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr get_gameObject_Injected(IntPtr _unity_self);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void GetComponentFastPath_Injected(
    IntPtr _unity_self,
    System.Type type,
    IntPtr oneFurtherThanResultValue);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr GetComponent_Injected(
    IntPtr _unity_self,
    ref ManagedSpanWrapper type);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void GetComponentsForListInternal_Injected(
    IntPtr _unity_self,
    System.Type searchType,
    object resultList);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern int GetComponentIndex_Injected(IntPtr _unity_self);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr GetCoupledComponent_Injected(IntPtr _unity_self);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool IsCoupledComponent_Injected(IntPtr _unity_self);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void SendMessageUpwards_Injected(
    IntPtr _unity_self,
    ref ManagedSpanWrapper methodName,
    [UnityEngine.Internal.DefaultValue("null")] object value,
    [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void SendMessage_Injected(
    IntPtr _unity_self,
    ref ManagedSpanWrapper methodName,
    object value,
    SendMessageOptions options);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void BroadcastMessage_Injected(
    IntPtr _unity_self,
    ref ManagedSpanWrapper methodName,
    [UnityEngine.Internal.DefaultValue("null")] object parameter,
    [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);
}
