// Decompiled with JetBrains decompiler
// Type: UnityEngine.Physics
// Assembly: UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DBEECE4C-E549-404C-B713-2AAB67D00EEC
// Assembly location: /home/jdw/Unity/Hub/Editor/6000.1.13f1/Editor/Data/Managed/UnityEngine/UnityEngine.PhysicsModule.dll
// XML documentation location: /home/jdw/Unity/Hub/Editor/6000.1.13f1/Editor/Data/Managed/UnityEngine/UnityEngine.PhysicsModule.xml

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

#nullable disable
namespace UnityEngine;

/// <summary>
///   <para>Global physics properties and helper methods.</para>
/// </summary>
[StaticAccessor("GetPhysicsManager()", StaticAccessorType.Dot)]
[NativeHeader("Modules/Physics/PhysicsQuery.h")]
[NativeHeader("Modules/Physics/PhysicsManager.h")]
public class Physics
{
  internal const float k_MaxFloatMinusEpsilon = 3.4028233E+38f;
  /// <summary>
  ///   <para>Layer mask constant to select ignore raycast layer.</para>
  /// </summary>
  public const int IgnoreRaycastLayer = 4;
  /// <summary>
  ///   <para>Layer mask constant to select default raycast layers.</para>
  /// </summary>
  public const int DefaultRaycastLayers = -5;
  /// <summary>
  ///   <para>Layer mask constant to select all layers.</para>
  /// </summary>
  public const int AllLayers = -1;
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Please use Physics.IgnoreRaycastLayer instead. (UnityUpgradable) -> IgnoreRaycastLayer", true)]
  public const int kIgnoreRaycastLayer = 4;
  [Obsolete("Please use Physics.DefaultRaycastLayers instead. (UnityUpgradable) -> DefaultRaycastLayers", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public const int kDefaultRaycastLayers = -5;
  [Obsolete("Please use Physics.AllLayers instead. (UnityUpgradable) -> AllLayers", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public const int kAllLayers = -1;
  private static readonly Collision s_ReusableCollision = new Collision();

  public static event Action<PhysicsScene, NativeArray<ModifiableContactPair>> ContactModifyEvent;

  public static event Action<PhysicsScene, NativeArray<ModifiableContactPair>> ContactModifyEventCCD;

  internal static event Action<PhysicsScene, IntPtr, int, bool> GenericContactModifyEvent = new Action<PhysicsScene, IntPtr, int, bool>(Physics.PhysXOnSceneContactModify);

  [RequiredByNativeCode]
  private static void OnSceneContactModify(
    PhysicsScene scene,
    IntPtr buffer,
    int count,
    bool isCCD)
  {
    Action<PhysicsScene, IntPtr, int, bool> contactModifyEvent = Physics.GenericContactModifyEvent;
    if (contactModifyEvent == null)
      return;
    contactModifyEvent(scene, buffer, count, isCCD);
  }

  private static unsafe void PhysXOnSceneContactModify(
    PhysicsScene scene,
    IntPtr buffer,
    int count,
    bool isCCD)
  {
    NativeArray<ModifiableContactPair> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ModifiableContactPair>(buffer.ToPointer(), count, Allocator.None);
    AtomicSafetyHandle atomicSafetyHandle = AtomicSafetyHandle.Create();
    NativeArrayUnsafeUtility.SetAtomicSafetyHandle<ModifiableContactPair>(ref nativeArray, atomicSafetyHandle);
    if (!isCCD)
    {
      Action<PhysicsScene, NativeArray<ModifiableContactPair>> contactModifyEvent = Physics.ContactModifyEvent;
      if (contactModifyEvent != null)
        contactModifyEvent(scene, nativeArray);
    }
    else
    {
      Action<PhysicsScene, NativeArray<ModifiableContactPair>> contactModifyEventCcd = Physics.ContactModifyEventCCD;
      if (contactModifyEventCcd != null)
        contactModifyEventCcd(scene, nativeArray);
    }
    AtomicSafetyHandle.Release(atomicSafetyHandle);
  }

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void GetIntegrationInfos(
    out IntPtr integrations,
    out ulong integrationCount);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void GetCurrentIntegrationInfo(out IntPtr integration);

  internal static unsafe ReadOnlySpan<IntegrationInfo> GetIntegrationInfos()
  {
    IntPtr integrations;
    ulong integrationCount;
    Physics.GetIntegrationInfos(out integrations, out integrationCount);
    return new ReadOnlySpan<IntegrationInfo>(integrations.ToPointer(), (int) integrationCount);
  }

  public static unsafe IntegrationInfo GetCurrentIntegrationInfo()
  {
    IntPtr integration;
    Physics.GetCurrentIntegrationInfo(out integration);
    return *(IntegrationInfo*) integration.ToPointer();
  }

  /// <summary>
  ///   <para>The gravity applied to all rigid bodies in the Scene.</para>
  /// </summary>
  public static Vector3 gravity
  {
    [ThreadSafe] get
    {
      Vector3 ret;
      Physics.get_gravity_Injected(out ret);
      return ret;
    }
    set => Physics.set_gravity_Injected(ref value);
  }

  /// <summary>
  ///   <para>The default contact offset of the newly created colliders.</para>
  /// </summary>
  public static extern float defaultContactOffset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>The mass-normalized energy threshold, below which objects start going to sleep.</para>
  /// </summary>
  public static extern float sleepThreshold { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Specifies whether queries (raycasts, spherecasts, overlap tests, etc.) hit Triggers by default.</para>
  /// </summary>
  public static extern bool queriesHitTriggers { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Whether physics queries should hit back-face triangles.</para>
  /// </summary>
  public static extern bool queriesHitBackfaces { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Two colliding objects with a relative velocity below this will not bounce (default 2). Must be positive.</para>
  /// </summary>
  public static extern float bounceThreshold { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>The maximum default velocity needed to move a Rigidbody's collider out of another collider's surface penetration. Must be positive.</para>
  /// </summary>
  public static extern float defaultMaxDepenetrationVelocity { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>The defaultSolverIterations determines how accurately Rigidbody joints and collision contacts are resolved. (default 6). Must be positive.</para>
  /// </summary>
  public static extern int defaultSolverIterations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>The defaultSolverVelocityIterations affects how accurately the Rigidbody joints and collision contacts are resolved. (default 1). Must be positive.</para>
  /// </summary>
  public static extern int defaultSolverVelocityIterations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Controls when Unity executes the physics simulation.</para>
  /// </summary>
  public static extern SimulationMode simulationMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Default maximum angular speed of the dynamic Rigidbody, in radians (default 50).</para>
  /// </summary>
  public static extern float defaultMaxAngularSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Enables an improved patch friction mode that guarantees static and dynamic friction do not exceed analytical results.</para>
  /// </summary>
  public static extern bool improvedPatchFriction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Whether or not MonoBehaviour collision messages will be sent by the physics system.</para>
  /// </summary>
  public static extern bool invokeCollisionCallbacks { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>The PhysicsScene automatically created when Unity starts.</para>
  /// </summary>
  [NativeProperty("DefaultPhysicsSceneHandle", true, TargetType.Function, true)]
  public static PhysicsScene defaultPhysicsScene
  {
    get
    {
      PhysicsScene ret;
      Physics.get_defaultPhysicsScene_Injected(out ret);
      return ret;
    }
  }

  /// <summary>
  ///   <para>Makes the collision detection system ignore all collisions between collider1 and collider2.</para>
  /// </summary>
  /// <param name="collider1">Any collider.</param>
  /// <param name="collider2">Another collider you want to have collider1 to start or stop ignoring collisions with.</param>
  /// <param name="ignore">Whether or not the collisions between the two colliders should be ignored or not.</param>
  public static void IgnoreCollision([NotNull] Collider collider1, [NotNull] Collider collider2, [UnityEngine.Internal.DefaultValue("true")] bool ignore)
  {
    if (collider1 == null)
      ThrowHelper.ThrowArgumentNullException((object) collider1, nameof (collider1));
    if (collider2 == null)
      ThrowHelper.ThrowArgumentNullException((object) collider2, nameof (collider2));
    IntPtr collider1_1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(collider1);
    if (collider1_1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) collider1, nameof (collider1));
    IntPtr collider2_1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(collider2);
    if (collider2_1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) collider2, nameof (collider2));
    Physics.IgnoreCollision_Injected(collider1_1, collider2_1, ignore);
  }

  [ExcludeFromDocs]
  public static void IgnoreCollision(Collider collider1, Collider collider2)
  {
    Physics.IgnoreCollision(collider1, collider2, true);
  }

  /// <summary>
  ///         <para>Makes the collision detection system ignore all collisions between any collider in layer1 and any collider in layer2.
  /// 
  /// Note that IgnoreLayerCollision will reset the trigger state of affected colliders, so you might receive OnTriggerExit and OnTriggerEnter messages in response to calling this.</para>
  ///       </summary>
  /// <param name="layer1"></param>
  /// <param name="layer2"></param>
  /// <param name="ignore"></param>
  [NativeName("IgnoreCollision")]
  [MethodImpl(MethodImplOptions.InternalCall)]
  public static extern void IgnoreLayerCollision(int layer1, int layer2, [UnityEngine.Internal.DefaultValue("true")] bool ignore);

  [ExcludeFromDocs]
  public static void IgnoreLayerCollision(int layer1, int layer2)
  {
    Physics.IgnoreLayerCollision(layer1, layer2, true);
  }

  /// <summary>
  ///   <para>Are collisions between layer1 and layer2 being ignored?</para>
  /// </summary>
  /// <param name="layer1"></param>
  /// <param name="layer2"></param>
  [MethodImpl(MethodImplOptions.InternalCall)]
  public static extern bool GetIgnoreLayerCollision(int layer1, int layer2);

  /// <summary>
  ///   <para>Checks whether the collision detection system will ignore all collisions/triggers between collider1 and collider2 or not.</para>
  /// </summary>
  /// <param name="collider1">The first collider to compare to collider2.</param>
  /// <param name="collider2">The second collider to compare to collider1.</param>
  /// <returns>
  ///   <para>Whether the collision detection system will ignore all collisions/triggers between collider1 and collider2 or not.</para>
  /// </returns>
  public static bool GetIgnoreCollision([NotNull] Collider collider1, [NotNull] Collider collider2)
  {
    if (collider1 == null)
      ThrowHelper.ThrowArgumentNullException((object) collider1, nameof (collider1));
    if (collider2 == null)
      ThrowHelper.ThrowArgumentNullException((object) collider2, nameof (collider2));
    IntPtr collider1_1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(collider1);
    if (collider1_1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) collider1, nameof (collider1));
    IntPtr collider2_1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(collider2);
    if (collider2_1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) collider2, nameof (collider2));
    return Physics.GetIgnoreCollision_Injected(collider1_1, collider2_1);
  }

  /// <summary>
  ///   <para>Casts a ray, from point origin, in direction direction, of length maxDistance, against all colliders in the Scene.</para>
  /// </summary>
  /// <param name="origin">The starting point of the ray in world coordinates.</param>
  /// <param name="direction">The direction of the ray.</param>
  /// <param name="maxDistance">The max distance the ray should check for collisions.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Returns true if the ray intersects with a Collider, otherwise false.</para>
  /// </returns>
  public static bool Raycast(
    Vector3 origin,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance, layerMask);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Vector3 origin, Vector3 direction)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction);
  }

  public static bool Raycast(
    Vector3 origin,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
  }

  [RequiredByNativeCode]
  [ExcludeFromDocs]
  public static bool Raycast(
    Vector3 origin,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance,
    int layerMask)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);
  }

  [ExcludeFromDocs]
  public static bool Raycast(
    Vector3 origin,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo);
  }

  /// <summary>
  ///   <para>Same as above using ray.origin and ray.direction instead of origin and direction.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray.</param>
  /// <param name="maxDistance">The max distance the ray should check for collisions.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Returns true when the ray intersects any collider, otherwise false.</para>
  /// </returns>
  public static bool Raycast(
    Ray ray,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray, float maxDistance, int layerMask)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance, layerMask);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray, float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction);
  }

  public static bool Raycast(
    Ray ray,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
  {
    return Physics.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance);
  }

  [ExcludeFromDocs]
  public static bool Raycast(Ray ray, out RaycastHit hitInfo)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo);
  }

  /// <summary>
  ///   <para>Returns true if there is any collider intersecting the line between start and end.</para>
  /// </summary>
  /// <param name="start">Start point.</param>
  /// <param name="end">End point.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  public static bool Linecast(
    Vector3 start,
    Vector3 end,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    Vector3 direction = end - start;
    return Physics.defaultPhysicsScene.Raycast(start, direction, direction.magnitude, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool Linecast(Vector3 start, Vector3 end, int layerMask)
  {
    return Physics.Linecast(start, end, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool Linecast(Vector3 start, Vector3 end)
  {
    return Physics.Linecast(start, end, -5, QueryTriggerInteraction.UseGlobal);
  }

  public static bool Linecast(
    Vector3 start,
    Vector3 end,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    Vector3 direction = end - start;
    return Physics.defaultPhysicsScene.Raycast(start, direction, out hitInfo, direction.magnitude, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask)
  {
    return Physics.Linecast(start, end, out hitInfo, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo)
  {
    return Physics.Linecast(start, end, out hitInfo, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Casts a capsule against all colliders in the Scene and returns detailed information on what was hit.</para>
  /// </summary>
  /// <param name="point1">The center of the sphere at the start of the capsule.</param>
  /// <param name="point2">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="direction">The direction into which to sweep the capsule.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>True if the capsule sweep intersects any collider, otherwise false.</para>
  /// </returns>
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, out RaycastHit _, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    float maxDistance,
    int layerMask)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    float maxDistance)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance,
    int layerMask)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CapsuleCast(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo)
  {
    return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  public static bool SphereCast(
    Vector3 origin,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(
    Vector3 origin,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(
    Vector3 origin,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo,
    float maxDistance)
  {
    return Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(
    Vector3 origin,
    float radius,
    Vector3 direction,
    out RaycastHit hitInfo)
  {
    return Physics.SphereCast(origin, radius, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Casts a sphere along a ray and returns detailed information on what was hit.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
  /// <param name="radius">The radius of the sphere.</param>
  /// <param name="maxDistance">The max length of the cast.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>True when the sphere sweep intersects any collider, otherwise false.</para>
  /// </returns>
  public static bool SphereCast(
    Ray ray,
    float radius,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.SphereCast(ray.origin, radius, ray.direction, out RaycastHit _, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(Ray ray, float radius, float maxDistance, int layerMask)
  {
    return Physics.SphereCast(ray, radius, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(Ray ray, float radius, float maxDistance)
  {
    return Physics.SphereCast(ray, radius, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(Ray ray, float radius)
  {
    return Physics.SphereCast(ray, radius, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  public static bool SphereCast(
    Ray ray,
    float radius,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.SphereCast(ray.origin, radius, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(
    Ray ray,
    float radius,
    out RaycastHit hitInfo,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance)
  {
    return Physics.SphereCast(ray, radius, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo)
  {
    return Physics.SphereCast(ray, radius, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Casts the box along a ray and returns detailed information on what was hit.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half the size of the box in each dimension.</param>
  /// <param name="direction">The direction in which to cast the box.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="maxDistance">The max length of the cast.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>True, if any intersections were found.</para>
  /// </returns>
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, out RaycastHit _, orientation, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation,
    float maxDistance,
    int layerMask)
  {
    return Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation,
    float maxDistance)
  {
    return Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation)
  {
    return Physics.BoxCast(center, halfExtents, direction, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction)
  {
    return Physics.BoxCast(center, halfExtents, direction, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    out RaycastHit hitInfo,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    out RaycastHit hitInfo,
    Quaternion orientation,
    float maxDistance,
    int layerMask)
  {
    return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    out RaycastHit hitInfo,
    Quaternion orientation,
    float maxDistance)
  {
    return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    out RaycastHit hitInfo,
    Quaternion orientation)
  {
    return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool BoxCast(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    out RaycastHit hitInfo)
  {
    return Physics.BoxCast(center, halfExtents, direction, out hitInfo, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::RaycastAll")]
  private static RaycastHit[] Internal_RaycastAll(
    PhysicsScene physicsScene,
    Ray ray,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    BlittableArrayWrapper ret;
    RaycastHit[] raycastHitArray;
    try
    {
      Physics.Internal_RaycastAll_Injected(ref physicsScene, ref ray, maxDistance, mask, queryTriggerInteraction, out ret);
    }
    finally
    {
      RaycastHit[] array;
      ret.Unmarshal<RaycastHit>(ref array);
      raycastHitArray = array;
    }
    return raycastHitArray;
  }

  /// <summary>
  ///   <para>Additional resources: Raycast.</para>
  /// </summary>
  /// <param name="origin">The starting point of the ray in world coordinates.</param>
  /// <param name="direction">The direction of the ray.</param>
  /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  public static RaycastHit[] RaycastAll(
    Vector3 origin,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    float magnitude = direction.magnitude;
    if ((double) magnitude <= 1.401298464324817E-45)
      return new RaycastHit[0];
    Vector3 direction1 = direction / magnitude;
    return Physics.Internal_RaycastAll(Physics.defaultPhysicsScene, new Ray(origin, direction1), maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(
    Vector3 origin,
    Vector3 direction,
    float maxDistance,
    int layerMask)
  {
    return Physics.RaycastAll(origin, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance)
  {
    return Physics.RaycastAll(origin, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction)
  {
    return Physics.RaycastAll(origin, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Casts a ray through the Scene and returns all hits. Note that order of the results is undefined.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray.</param>
  /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>An array of RaycastHit objects. Note that the order of the results is undefined.</para>
  /// </returns>
  public static RaycastHit[] RaycastAll(
    Ray ray,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
  }

  [RequiredByNativeCode]
  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask)
  {
    return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(Ray ray, float maxDistance)
  {
    return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] RaycastAll(Ray ray)
  {
    return Physics.RaycastAll(ray.origin, ray.direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Cast a ray through the Scene and store the hits into the buffer.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray.</param>
  /// <param name="results">The buffer to store the hits into.</param>
  /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>The amount of hits stored into the results buffer.</para>
  /// </returns>
  public static int RaycastNonAlloc(
    Ray ray,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance, layerMask, queryTriggerInteraction);
  }

  [RequiredByNativeCode]
  [ExcludeFromDocs]
  public static int RaycastNonAlloc(
    Ray ray,
    RaycastHit[] results,
    float maxDistance,
    int layerMask)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance, layerMask);
  }

  [ExcludeFromDocs]
  public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance);
  }

  [ExcludeFromDocs]
  public static int RaycastNonAlloc(Ray ray, RaycastHit[] results)
  {
    return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results);
  }

  /// <summary>
  ///   <para>Cast a ray through the Scene and store the hits into the buffer.</para>
  /// </summary>
  /// <param name="origin">The starting point and direction of the ray.</param>
  /// <param name="results">The buffer to store the hits into.</param>
  /// <param name="direction">The direction of the ray.</param>
  /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <returns>
  ///   <para>The amount of hits stored into the results buffer.</para>
  /// </returns>
  public static int RaycastNonAlloc(
    Vector3 origin,
    Vector3 direction,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int RaycastNonAlloc(
    Vector3 origin,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance,
    int layerMask)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance, layerMask);
  }

  [ExcludeFromDocs]
  public static int RaycastNonAlloc(
    Vector3 origin,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance);
  }

  [ExcludeFromDocs]
  public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results)
  {
    return Physics.defaultPhysicsScene.Raycast(origin, direction, results);
  }

  [FreeFunction("Physics::CapsuleCastAll")]
  private static RaycastHit[] Query_CapsuleCastAll(
    PhysicsScene physicsScene,
    Vector3 p0,
    Vector3 p1,
    float radius,
    Vector3 direction,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    BlittableArrayWrapper ret;
    RaycastHit[] raycastHitArray;
    try
    {
      Physics.Query_CapsuleCastAll_Injected(ref physicsScene, ref p0, ref p1, radius, ref direction, maxDistance, mask, queryTriggerInteraction, out ret);
    }
    finally
    {
      RaycastHit[] array;
      ret.Unmarshal<RaycastHit>(ref array);
      raycastHitArray = array;
    }
    return raycastHitArray;
  }

  /// <summary>
  ///   <para>Like Physics.CapsuleCast, but this function will return all hits the capsule sweep intersects.</para>
  /// </summary>
  /// <param name="point1">The center of the sphere at the start of the capsule.</param>
  /// <param name="point2">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="direction">The direction into which to sweep the capsule.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layermask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layerMask"></param>
  /// <returns>
  ///   <para>An array of all colliders hit in the sweep.</para>
  /// </returns>
  public static RaycastHit[] CapsuleCastAll(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    float magnitude = direction.magnitude;
    if ((double) magnitude <= 1.401298464324817E-45)
      return new RaycastHit[0];
    Vector3 direction1 = direction / magnitude;
    return Physics.Query_CapsuleCastAll(Physics.defaultPhysicsScene, point1, point2, radius, direction1, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] CapsuleCastAll(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    float maxDistance,
    int layerMask)
  {
    return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] CapsuleCastAll(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    float maxDistance)
  {
    return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] CapsuleCastAll(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction)
  {
    return Physics.CapsuleCastAll(point1, point2, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::SphereCastAll")]
  private static RaycastHit[] Query_SphereCastAll(
    PhysicsScene physicsScene,
    Vector3 origin,
    float radius,
    Vector3 direction,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    BlittableArrayWrapper ret;
    RaycastHit[] raycastHitArray;
    try
    {
      Physics.Query_SphereCastAll_Injected(ref physicsScene, ref origin, radius, ref direction, maxDistance, mask, queryTriggerInteraction, out ret);
    }
    finally
    {
      RaycastHit[] array;
      ret.Unmarshal<RaycastHit>(ref array);
      raycastHitArray = array;
    }
    return raycastHitArray;
  }

  /// <summary>
  ///   <para>Like Physics.SphereCast, but this function will return all hits the sphere sweep intersects.</para>
  /// </summary>
  /// <param name="origin">The center of the sphere at the start of the sweep.</param>
  /// <param name="radius">The radius of the sphere.</param>
  /// <param name="direction">The direction in which to sweep the sphere.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>An array of all colliders hit in the sweep.</para>
  /// </returns>
  public static RaycastHit[] SphereCastAll(
    Vector3 origin,
    float radius,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    float magnitude = direction.magnitude;
    if ((double) magnitude <= 1.401298464324817E-45)
      return new RaycastHit[0];
    Vector3 direction1 = direction / magnitude;
    return Physics.Query_SphereCastAll(Physics.defaultPhysicsScene, origin, radius, direction1, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(
    Vector3 origin,
    float radius,
    Vector3 direction,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(
    Vector3 origin,
    float radius,
    Vector3 direction,
    float maxDistance)
  {
    return Physics.SphereCastAll(origin, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction)
  {
    return Physics.SphereCastAll(origin, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Like Physics.SphereCast, but this function will return all hits the sphere sweep intersects.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
  /// <param name="radius">The radius of the sphere.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  public static RaycastHit[] SphereCastAll(
    Ray ray,
    float radius,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.SphereCastAll(ray.origin, radius, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(
    Ray ray,
    float radius,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCastAll(ray, radius, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance)
  {
    return Physics.SphereCastAll(ray, radius, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] SphereCastAll(Ray ray, float radius)
  {
    return Physics.SphereCastAll(ray, radius, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::OverlapCapsule")]
  private static Collider[] OverlapCapsule_Internal(
    PhysicsScene physicsScene,
    Vector3 point0,
    Vector3 point1,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapCapsule_Internal_Injected(ref physicsScene, ref point0, ref point1, radius, layerMask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Check the given capsule against the physics world and return all overlapping colliders.</para>
  /// </summary>
  /// <param name="point0">The center of the sphere at the start of the capsule.</param>
  /// <param name="point1">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Colliders touching or inside the capsule.</para>
  /// </returns>
  public static Collider[] OverlapCapsule(
    Vector3 point0,
    Vector3 point1,
    float radius,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapCapsule_Internal(Physics.defaultPhysicsScene, point0, point1, radius, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapCapsule(
    Vector3 point0,
    Vector3 point1,
    float radius,
    int layerMask)
  {
    return Physics.OverlapCapsule(point0, point1, radius, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius)
  {
    return Physics.OverlapCapsule(point0, point1, radius, -1, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::OverlapSphere")]
  private static Collider[] OverlapSphere_Internal(
    PhysicsScene physicsScene,
    Vector3 position,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapSphere_Internal_Injected(ref physicsScene, ref position, radius, layerMask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Computes and stores colliders touching or inside the sphere.</para>
  /// </summary>
  /// <param name="position">Center of the sphere.</param>
  /// <param name="radius">Radius of the sphere.</param>
  /// <param name="layerMask">A defines which layers of colliders to include in the query.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Returns an array with all colliders touching or inside the sphere.</para>
  /// </returns>
  public static Collider[] OverlapSphere(
    Vector3 position,
    float radius,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapSphere_Internal(Physics.defaultPhysicsScene, position, radius, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask)
  {
    return Physics.OverlapSphere(position, radius, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapSphere(Vector3 position, float radius)
  {
    return Physics.OverlapSphere(position, radius, -1, QueryTriggerInteraction.UseGlobal);
  }

  [NativeName("Simulate")]
  internal static void Simulate_Internal(
    PhysicsScene physicsScene,
    float step,
    SimulationStage stages,
    SimulationOption options)
  {
    Physics.Simulate_Internal_Injected(ref physicsScene, step, stages, options);
  }

  /// <summary>
  ///   <para>Simulate physics in the Scene.</para>
  /// </summary>
  /// <param name="step">The time to advance physics by.</param>
  public static void Simulate(float step)
  {
    if (Physics.simulationMode != SimulationMode.Script)
      Debug.LogWarning((object) "Physics.Simulate(...) was called but simulation mode is not set to Script. You should set simulation mode to Script first before calling this function therefore the simulation was not run.");
    else
      Physics.Simulate_Internal(Physics.defaultPhysicsScene, step, SimulationStage.All, SimulationOption.All);
  }

  [NativeName("InterpolateBodies")]
  internal static void InterpolateBodies_Internal(PhysicsScene physicsScene)
  {
    Physics.InterpolateBodies_Internal_Injected(ref physicsScene);
  }

  [NativeName("ResetInterpolatedTransformPosition")]
  internal static void ResetInterpolationPoses_Internal(PhysicsScene physicsScene)
  {
    Physics.ResetInterpolationPoses_Internal_Injected(ref physicsScene);
  }

  /// <summary>
  ///   <para>Apply Transform changes to the physics engine.</para>
  /// </summary>
  [MethodImpl(MethodImplOptions.InternalCall)]
  public static extern void SyncTransforms();

  /// <summary>
  ///   <para>Whether or not to automatically sync transform changes with the physics system whenever a Transform component changes.</para>
  /// </summary>
  public static extern bool autoSyncTransforms { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Determines whether the garbage collector should reuse only a single instance of a Collision type for all collision callbacks.</para>
  /// </summary>
  public static extern bool reuseCollisionCallbacks { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

  [FreeFunction("Physics::ComputePenetration")]
  private static bool Query_ComputePenetration(
    [NotNull] Collider colliderA,
    Vector3 positionA,
    Quaternion rotationA,
    [NotNull] Collider colliderB,
    Vector3 positionB,
    Quaternion rotationB,
    ref Vector3 direction,
    ref float distance)
  {
    if (colliderA == null)
      ThrowHelper.ThrowArgumentNullException((object) colliderA, nameof (colliderA));
    if (colliderB == null)
      ThrowHelper.ThrowArgumentNullException((object) colliderB, nameof (colliderB));
    IntPtr colliderA1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(colliderA);
    if (colliderA1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) colliderA, nameof (colliderA));
    ref Vector3 local1 = ref positionA;
    ref Quaternion local2 = ref rotationA;
    IntPtr colliderB1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(colliderB);
    if (colliderB1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) colliderB, nameof (colliderB));
    ref Vector3 local3 = ref positionB;
    ref Quaternion local4 = ref rotationB;
    ref Vector3 local5 = ref direction;
    ref float local6 = ref distance;
    return Physics.Query_ComputePenetration_Injected(colliderA1, ref local1, ref local2, colliderB1, ref local3, ref local4, ref local5, ref local6);
  }

  public static bool ComputePenetration(
    Collider colliderA,
    Vector3 positionA,
    Quaternion rotationA,
    Collider colliderB,
    Vector3 positionB,
    Quaternion rotationB,
    out Vector3 direction,
    out float distance)
  {
    direction = Vector3.zero;
    distance = 0.0f;
    return Physics.Query_ComputePenetration(colliderA, positionA, rotationA, colliderB, positionB, rotationB, ref direction, ref distance);
  }

  [FreeFunction("Physics::ClosestPoint")]
  private static Vector3 Query_ClosestPoint(
    [NotNull] Collider collider,
    Vector3 position,
    Quaternion rotation,
    Vector3 point)
  {
    if (collider == null)
      ThrowHelper.ThrowArgumentNullException((object) collider, nameof (collider));
    IntPtr collider1 = Object.MarshalledUnityObject.MarshalNotNull<Collider>(collider);
    if (collider1 == IntPtr.Zero)
      ThrowHelper.ThrowArgumentNullException((object) collider, nameof (collider));
    Vector3 ret;
    Physics.Query_ClosestPoint_Injected(collider1, ref position, ref rotation, ref point, out ret);
    return ret;
  }

  /// <summary>
  ///   <para>Returns a point on the given collider that is closest to the specified location.</para>
  /// </summary>
  /// <param name="point">Location you want to find the closest point to.</param>
  /// <param name="collider">The collider that you find the closest point on.</param>
  /// <param name="position">The position of the collider.</param>
  /// <param name="rotation">The rotation of the collider.</param>
  /// <returns>
  ///   <para>The point on the collider that is closest to the specified location.</para>
  /// </returns>
  public static Vector3 ClosestPoint(
    Vector3 point,
    Collider collider,
    Vector3 position,
    Quaternion rotation)
  {
    return Physics.Query_ClosestPoint(collider, position, rotation, point);
  }

  /// <summary>
  ///   <para>Sets the minimum separation distance for cloth inter-collision.</para>
  /// </summary>
  [StaticAccessor("GetPhysicsManager()")]
  public static extern float interCollisionDistance { [NativeName("GetClothInterCollisionDistance"), MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("SetClothInterCollisionDistance"), MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///   <para>Sets the cloth inter-collision stiffness.</para>
  /// </summary>
  [StaticAccessor("GetPhysicsManager()")]
  public static extern float interCollisionStiffness { [NativeName("GetClothInterCollisionStiffness"), MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("SetClothInterCollisionStiffness"), MethodImpl(MethodImplOptions.InternalCall)] set; }

  [StaticAccessor("GetPhysicsManager()")]
  public static extern bool interCollisionSettingsToggle { [NativeName("GetClothInterCollisionSettingsToggle"), MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("SetClothInterCollisionSettingsToggle"), MethodImpl(MethodImplOptions.InternalCall)] set; }

  /// <summary>
  ///         <para>Cloth Gravity setting.
  /// Set gravity for all cloth components.</para>
  ///       </summary>
  public static Vector3 clothGravity
  {
    [ThreadSafe] get
    {
      Vector3 ret;
      Physics.get_clothGravity_Injected(out ret);
      return ret;
    }
    set => Physics.set_clothGravity_Injected(ref value);
  }

  /// <summary>
  ///   <para>Computes and stores colliders touching or inside the sphere into the provided buffer.</para>
  /// </summary>
  /// <param name="position">Center of the sphere.</param>
  /// <param name="radius">Radius of the sphere.</param>
  /// <param name="results">The buffer to store the results into.</param>
  /// <param name="layerMask">A defines which layers of colliders to include in the query.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Returns the amount of colliders stored into the results buffer.</para>
  /// </returns>
  public static int OverlapSphereNonAlloc(
    Vector3 position,
    float radius,
    Collider[] results,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.OverlapSphere(position, radius, results, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int OverlapSphereNonAlloc(
    Vector3 position,
    float radius,
    Collider[] results,
    int layerMask)
  {
    return Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results)
  {
    return Physics.OverlapSphereNonAlloc(position, radius, results, -1, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::SphereTest")]
  private static bool CheckSphere_Internal(
    PhysicsScene physicsScene,
    Vector3 position,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckSphere_Internal_Injected(ref physicsScene, ref position, radius, layerMask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Returns true if there are any colliders overlapping the sphere defined by position and radius in world coordinates.</para>
  /// </summary>
  /// <param name="position">Center of the sphere.</param>
  /// <param name="radius">Radius of the sphere.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  public static bool CheckSphere(
    Vector3 position,
    float radius,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckSphere_Internal(Physics.defaultPhysicsScene, position, radius, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool CheckSphere(Vector3 position, float radius, int layerMask)
  {
    return Physics.CheckSphere(position, radius, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CheckSphere(Vector3 position, float radius)
  {
    return Physics.CheckSphere(position, radius, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Casts a capsule against all colliders in the Scene and returns detailed information on what was hit into the buffer.</para>
  /// </summary>
  /// <param name="point1">The center of the sphere at the start of the capsule.</param>
  /// <param name="point2">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="direction">The direction into which to sweep the capsule.</param>
  /// <param name="results">The buffer to store the hits into.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>The amount of hits stored into the buffer.</para>
  /// </returns>
  public static int CapsuleCastNonAlloc(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int CapsuleCastNonAlloc(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance,
    int layerMask)
  {
    return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int CapsuleCastNonAlloc(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance)
  {
    return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int CapsuleCastNonAlloc(
    Vector3 point1,
    Vector3 point2,
    float radius,
    Vector3 direction,
    RaycastHit[] results)
  {
    return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Cast sphere along the direction and store the results into buffer.</para>
  /// </summary>
  /// <param name="origin">The center of the sphere at the start of the sweep.</param>
  /// <param name="radius">The radius of the sphere.</param>
  /// <param name="direction">The direction in which to sweep the sphere.</param>
  /// <param name="results">The buffer to save the hits into.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>The amount of hits stored into the results buffer.</para>
  /// </returns>
  public static int SphereCastNonAlloc(
    Vector3 origin,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.SphereCast(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(
    Vector3 origin,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(
    Vector3 origin,
    float radius,
    Vector3 direction,
    RaycastHit[] results,
    float maxDistance)
  {
    return Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(
    Vector3 origin,
    float radius,
    Vector3 direction,
    RaycastHit[] results)
  {
    return Physics.SphereCastNonAlloc(origin, radius, direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Cast sphere along the direction and store the results into buffer.</para>
  /// </summary>
  /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
  /// <param name="radius">The radius of the sphere.</param>
  /// <param name="results">The buffer to save the results to.</param>
  /// <param name="maxDistance">The max length of the sweep.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>The amount of hits stored into the results buffer.</para>
  /// </returns>
  public static int SphereCastNonAlloc(
    Ray ray,
    float radius,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.SphereCastNonAlloc(ray.origin, radius, ray.direction, results, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(
    Ray ray,
    float radius,
    RaycastHit[] results,
    float maxDistance,
    int layerMask)
  {
    return Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(
    Ray ray,
    float radius,
    RaycastHit[] results,
    float maxDistance)
  {
    return Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results)
  {
    return Physics.SphereCastNonAlloc(ray, radius, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::CapsuleTest")]
  private static bool CheckCapsule_Internal(
    PhysicsScene physicsScene,
    Vector3 start,
    Vector3 end,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckCapsule_Internal_Injected(ref physicsScene, ref start, ref end, radius, layerMask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Checks if any colliders overlap a capsule-shaped volume in world space.</para>
  /// </summary>
  /// <param name="start">The center of the sphere at the start of the capsule.</param>
  /// <param name="end">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="layermask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layerMask"></param>
  public static bool CheckCapsule(
    Vector3 start,
    Vector3 end,
    float radius,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckCapsule_Internal(Physics.defaultPhysicsScene, start, end, radius, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layerMask)
  {
    return Physics.CheckCapsule(start, end, radius, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CheckCapsule(Vector3 start, Vector3 end, float radius)
  {
    return Physics.CheckCapsule(start, end, radius, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::BoxTest")]
  private static bool CheckBox_Internal(
    PhysicsScene physicsScene,
    Vector3 center,
    Vector3 halfExtents,
    Quaternion orientation,
    int layermask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckBox_Internal_Injected(ref physicsScene, ref center, ref halfExtents, ref orientation, layermask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Check whether the given box overlaps with other colliders or not.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half the size of the box in each dimension.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layermask"></param>
  /// <returns>
  ///   <para>True, if the box overlaps with any colliders.</para>
  /// </returns>
  public static bool CheckBox(
    Vector3 center,
    Vector3 halfExtents,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layermask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.CheckBox_Internal(Physics.defaultPhysicsScene, center, halfExtents, orientation, layermask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static bool CheckBox(
    Vector3 center,
    Vector3 halfExtents,
    Quaternion orientation,
    int layerMask)
  {
    return Physics.CheckBox(center, halfExtents, orientation, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
  {
    return Physics.CheckBox(center, halfExtents, orientation, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static bool CheckBox(Vector3 center, Vector3 halfExtents)
  {
    return Physics.CheckBox(center, halfExtents, Quaternion.identity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::OverlapBox")]
  private static Collider[] OverlapBox_Internal(
    PhysicsScene physicsScene,
    Vector3 center,
    Vector3 halfExtents,
    Quaternion orientation,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapBox_Internal_Injected(ref physicsScene, ref center, ref halfExtents, ref orientation, layerMask, queryTriggerInteraction);
  }

  /// <summary>
  ///   <para>Find all colliders touching or inside of the given box.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half of the size of the box in each dimension.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>Colliders that overlap with the given box.</para>
  /// </returns>
  public static Collider[] OverlapBox(
    Vector3 center,
    Vector3 halfExtents,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.OverlapBox_Internal(Physics.defaultPhysicsScene, center, halfExtents, orientation, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapBox(
    Vector3 center,
    Vector3 halfExtents,
    Quaternion orientation,
    int layerMask)
  {
    return Physics.OverlapBox(center, halfExtents, orientation, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
  {
    return Physics.OverlapBox(center, halfExtents, orientation, -1, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents)
  {
    return Physics.OverlapBox(center, halfExtents, Quaternion.identity, -1, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Find all colliders touching or inside of the given box, and store them into the buffer.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half of the size of the box in each dimension.</param>
  /// <param name="results">The buffer to store the results in.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="layerMask">A that is used to selectively filter which colliders are considered when casting a ray.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="mask"></param>
  /// <returns>
  ///   <para>The amount of colliders stored in results.</para>
  /// </returns>
  public static int OverlapBoxNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Collider[] results,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int mask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.OverlapBox(center, halfExtents, results, orientation, mask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int OverlapBoxNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Collider[] results,
    Quaternion orientation,
    int mask)
  {
    return Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, mask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int OverlapBoxNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Collider[] results,
    Quaternion orientation)
  {
    return Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, -1, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results)
  {
    return Physics.OverlapBoxNonAlloc(center, halfExtents, results, Quaternion.identity, -1, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Cast the box along the direction, and store hits in the provided buffer.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half the size of the box in each dimension.</param>
  /// <param name="direction">The direction in which to cast the box.</param>
  /// <param name="results">The buffer to store the results in.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="maxDistance">The max length of the cast.</param>
  /// <param name="layermask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layerMask"></param>
  /// <returns>
  ///   <para>The amount of hits stored to the results buffer.</para>
  /// </returns>
  public static int BoxCastNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    RaycastHit[] results,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int BoxCastNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    RaycastHit[] results,
    Quaternion orientation)
  {
    return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int BoxCastNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    RaycastHit[] results,
    Quaternion orientation,
    float maxDistance)
  {
    return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int BoxCastNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    RaycastHit[] results,
    Quaternion orientation,
    float maxDistance,
    int layerMask)
  {
    return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int BoxCastNonAlloc(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    RaycastHit[] results)
  {
    return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [FreeFunction("Physics::BoxCastAll")]
  private static RaycastHit[] Internal_BoxCastAll(
    PhysicsScene physicsScene,
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation,
    float maxDistance,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction)
  {
    BlittableArrayWrapper ret;
    RaycastHit[] raycastHitArray;
    try
    {
      Physics.Internal_BoxCastAll_Injected(ref physicsScene, ref center, ref halfExtents, ref direction, ref orientation, maxDistance, layerMask, queryTriggerInteraction, out ret);
    }
    finally
    {
      RaycastHit[] array;
      ret.Unmarshal<RaycastHit>(ref array);
      raycastHitArray = array;
    }
    return raycastHitArray;
  }

  /// <summary>
  ///   <para>Like Physics.BoxCast, but returns all hits.</para>
  /// </summary>
  /// <param name="center">Center of the box.</param>
  /// <param name="halfExtents">Half the size of the box in each dimension.</param>
  /// <param name="direction">The direction in which to cast the box.</param>
  /// <param name="orientation">Rotation of the box.</param>
  /// <param name="maxDistance">The max length of the cast.</param>
  /// <param name="layermask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <param name="layerMask"></param>
  /// <returns>
  ///   <para>All colliders that were hit.</para>
  /// </returns>
  public static RaycastHit[] BoxCastAll(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    [UnityEngine.Internal.DefaultValue("Quaternion.identity")] Quaternion orientation,
    [UnityEngine.Internal.DefaultValue("Mathf.Infinity")] float maxDistance,
    [UnityEngine.Internal.DefaultValue("DefaultRaycastLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    float magnitude = direction.magnitude;
    if ((double) magnitude <= 1.401298464324817E-45)
      return new RaycastHit[0];
    Vector3 direction1 = direction / magnitude;
    return Physics.Internal_BoxCastAll(Physics.defaultPhysicsScene, center, halfExtents, direction1, orientation, maxDistance, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] BoxCastAll(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation,
    float maxDistance,
    int layerMask)
  {
    return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] BoxCastAll(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation,
    float maxDistance)
  {
    return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] BoxCastAll(
    Vector3 center,
    Vector3 halfExtents,
    Vector3 direction,
    Quaternion orientation)
  {
    return Physics.BoxCastAll(center, halfExtents, direction, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction)
  {
    return Physics.BoxCastAll(center, halfExtents, direction, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Check the given capsule against the physics world and return all overlapping colliders in the user-provided buffer.</para>
  /// </summary>
  /// <param name="point0">The center of the sphere at the start of the capsule.</param>
  /// <param name="point1">The center of the sphere at the end of the capsule.</param>
  /// <param name="radius">The radius of the capsule.</param>
  /// <param name="results">The buffer to store the results into.</param>
  /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
  /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
  /// <returns>
  ///   <para>The amount of entries written to the buffer.</para>
  /// </returns>
  public static int OverlapCapsuleNonAlloc(
    Vector3 point0,
    Vector3 point1,
    float radius,
    Collider[] results,
    [UnityEngine.Internal.DefaultValue("AllLayers")] int layerMask,
    [UnityEngine.Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
  {
    return Physics.defaultPhysicsScene.OverlapCapsule(point0, point1, radius, results, layerMask, queryTriggerInteraction);
  }

  [ExcludeFromDocs]
  public static int OverlapCapsuleNonAlloc(
    Vector3 point0,
    Vector3 point1,
    float radius,
    Collider[] results,
    int layerMask)
  {
    return Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, QueryTriggerInteraction.UseGlobal);
  }

  [ExcludeFromDocs]
  public static int OverlapCapsuleNonAlloc(
    Vector3 point0,
    Vector3 point1,
    float radius,
    Collider[] results)
  {
    return Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, -1, QueryTriggerInteraction.UseGlobal);
  }

  /// <summary>
  ///   <para>Prepares the mesh for use with a MeshCollider.</para>
  /// </summary>
  /// <param name="meshID">The instance ID of the mesh to bake collision data from.</param>
  /// <param name="convex">A flag to indicate whether to bake convex geometry or not.</param>
  /// <param name="cookingOptions">The cooking options to use when you bake the mesh.</param>
  [ThreadSafe]
  [StaticAccessor("GetPhysicsManager()")]
  [MethodImpl(MethodImplOptions.InternalCall)]
  public static extern void BakeMesh(
    int meshID,
    bool convex,
    MeshColliderCookingOptions cookingOptions);

  /// <summary>
  ///   <para>Prepares the mesh for use with a MeshCollider and uses default cooking options.</para>
  /// </summary>
  /// <param name="meshID">The instance ID of the mesh to bake collision data from.</param>
  /// <param name="convex">A flag to indicate whether to bake convex geometry or not.</param>
  public static void BakeMesh(int meshID, bool convex)
  {
    Physics.BakeMesh(meshID, convex, MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase);
  }

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  [MethodImpl(MethodImplOptions.InternalCall)]
  internal static extern bool ConnectPhysicsSDKVisualDebugger();

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  [MethodImpl(MethodImplOptions.InternalCall)]
  internal static extern void DisconnectPhysicsSDKVisualDebugger();

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  internal static Collider GetColliderByInstanceID(int instanceID)
  {
    return Unmarshal.UnmarshalUnityObject<Collider>(Physics.GetColliderByInstanceID_Injected(instanceID));
  }

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  internal static Component GetBodyByInstanceID(int instanceID)
  {
    return Unmarshal.UnmarshalUnityObject<Component>(Physics.GetBodyByInstanceID_Injected(instanceID));
  }

  [ThreadSafe]
  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  [MethodImpl(MethodImplOptions.InternalCall)]
  internal static extern uint TranslateTriangleIndexFromID(int instanceID, uint faceIndex);

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  private static void SendOnCollisionEnter(Component component, Collision collision)
  {
    Physics.SendOnCollisionEnter_Injected(Object.MarshalledUnityObject.Marshal<Component>(component), collision);
  }

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  private static void SendOnCollisionStay(Component component, Collision collision)
  {
    Physics.SendOnCollisionStay_Injected(Object.MarshalledUnityObject.Marshal<Component>(component), collision);
  }

  [StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
  private static void SendOnCollisionExit(Component component, Collision collision)
  {
    Physics.SendOnCollisionExit_Injected(Object.MarshalledUnityObject.Marshal<Component>(component), collision);
  }

  /// <summary>
  ///   <para>The minimum contact penetration value in order to apply a penalty force (default 0.05). Must be positive.</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Use Physics.defaultContactOffset or Collider.contactOffset instead.", true)]
  public static float minPenetrationForPenalty
  {
    get => 0.0f;
    set
    {
    }
  }

  [Obsolete("Please use bounceThreshold instead. (UnityUpgradable) -> bounceThreshold")]
  public static float bounceTreshold
  {
    get => Physics.bounceThreshold;
    set => Physics.bounceThreshold = value;
  }

  /// <summary>
  ///   <para>The default linear velocity, below which objects start going to sleep (default 0.15). Must be positive.</para>
  /// </summary>
  [Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static float sleepVelocity
  {
    get => 0.0f;
    set
    {
    }
  }

  /// <summary>
  ///   <para>The default angular velocity, below which objects start sleeping (default 0.14). Must be positive.</para>
  /// </summary>
  [Obsolete("The sleepAngularVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static float sleepAngularVelocity
  {
    get => 0.0f;
    set
    {
    }
  }

  /// <summary>
  ///   <para>The default maximum angular velocity permitted for any rigid bodies (default 7). Must be positive.</para>
  /// </summary>
  [Obsolete("Use Rigidbody.maxAngularVelocity instead.", true)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static float maxAngularVelocity
  {
    get => 0.0f;
    set
    {
    }
  }

  [Obsolete("Please use Physics.defaultSolverIterations instead. (UnityUpgradable) -> defaultSolverIterations")]
  public static int solverIterationCount
  {
    get => Physics.defaultSolverIterations;
    set => Physics.defaultSolverIterations = value;
  }

  [Obsolete("Please use Physics.defaultSolverVelocityIterations instead. (UnityUpgradable) -> defaultSolverVelocityIterations")]
  public static int solverVelocityIterationCount
  {
    get => Physics.defaultSolverVelocityIterations;
    set => Physics.defaultSolverVelocityIterations = value;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("penetrationPenaltyForce has no effect.", true)]
  public static float penetrationPenaltyForce
  {
    get => 0.0f;
    set
    {
    }
  }

  /// <summary>
  ///   <para>Sets whether the physics should be simulated automatically or not.</para>
  /// </summary>
  [Obsolete("Physics.autoSimulation has been replaced by Physics.simulationMode", false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool autoSimulation
  {
    get => Physics.simulationMode != SimulationMode.Script;
    set => Physics.simulationMode = value ? SimulationMode.FixedUpdate : SimulationMode.Script;
  }

  /// <summary>
  ///   <para>Rebuild the broadphase interest regions as well as set the world boundaries.</para>
  /// </summary>
  /// <param name="worldBounds">Boundaries of the physics world.</param>
  /// <param name="subdivisions">How many cells to create along x and z axis.</param>
  [Obsolete("Physics.RebuildBroadphaseRegions has been deprecated alongside Multi Box Pruning. Use Automatic Box Pruning instead.", false)]
  public static void RebuildBroadphaseRegions(Bounds worldBounds, int subdivisions)
  {
  }

  public static event Physics.ContactEventDelegate ContactEvent;

  [RequiredByNativeCode]
  private static unsafe void OnSceneContact(PhysicsScene scene, IntPtr buffer, int count)
  {
    if (count == 0)
      return;
    NativeArray<ContactPairHeader> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ContactPairHeader>(buffer.ToPointer(), count, Allocator.None);
    AtomicSafetyHandle atomicSafetyHandle = AtomicSafetyHandle.Create();
    NativeArrayUnsafeUtility.SetAtomicSafetyHandle<ContactPairHeader>(ref nativeArray, atomicSafetyHandle);
    Profiler.BeginSample("Physics.ContactEvent");
    try
    {
      Physics.ContactEventDelegate contactEvent = Physics.ContactEvent;
      if (contactEvent != null)
        contactEvent(scene, nativeArray.AsReadOnly());
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    finally
    {
      Profiler.EndSample();
      Physics.ReportContacts(nativeArray.AsReadOnly());
    }
    AtomicSafetyHandle.Release(atomicSafetyHandle);
  }

  private static void ReportContacts(NativeArray<ContactPairHeader>.ReadOnly array)
  {
    if (!Physics.invokeCollisionCallbacks)
      return;
    Profiler.BeginSample("Physics.InvokeOnCollisionEvents");
    for (int index1 = 0; index1 < array.Length; ++index1)
    {
      ContactPairHeader header = array[index1];
      if (!header.hasRemovedBody)
      {
        for (int index2 = 0; (long) index2 < (long) header.m_NbPairs; ++index2)
        {
          ref readonly ContactPair local = ref header.GetContactPair(index2);
          if (!local.hasRemovedCollider)
          {
            Component body = header.body;
            Component otherBody = header.otherBody;
            Component component1 = (Object) body != (Object) null ? body : (Component) local.collider;
            Component component2 = (Object) otherBody != (Object) null ? otherBody : (Component) local.otherCollider;
            if ((bool) (Object) component1 && (bool) (Object) component2)
            {
              if (local.isCollisionEnter)
              {
                Physics.SendOnCollisionEnter(component1, Physics.GetCollisionToReport(in header, in local, false));
                Physics.SendOnCollisionEnter(component2, Physics.GetCollisionToReport(in header, in local, true));
              }
              if (local.isCollisionStay)
              {
                Physics.SendOnCollisionStay(component1, Physics.GetCollisionToReport(in header, in local, false));
                Physics.SendOnCollisionStay(component2, Physics.GetCollisionToReport(in header, in local, true));
              }
              if (local.isCollisionExit)
              {
                Physics.SendOnCollisionExit(component1, Physics.GetCollisionToReport(in header, in local, false));
                Physics.SendOnCollisionExit(component2, Physics.GetCollisionToReport(in header, in local, true));
              }
            }
          }
        }
      }
    }
    Profiler.EndSample();
  }

  private static Collision GetCollisionToReport(
    in ContactPairHeader header,
    in ContactPair pair,
    bool flipped)
  {
    if (!Physics.reuseCollisionCallbacks)
      return new Collision(in header, in pair, flipped);
    Physics.s_ReusableCollision.Reuse(in header, in pair);
    Physics.s_ReusableCollision.Flipped = flipped;
    return Physics.s_ReusableCollision;
  }

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void get_gravity_Injected(out Vector3 ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void set_gravity_Injected([In] ref Vector3 value);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void get_defaultPhysicsScene_Injected(out PhysicsScene ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void IgnoreCollision_Injected(
    IntPtr collider1,
    IntPtr collider2,
    [UnityEngine.Internal.DefaultValue("true")] bool ignore);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool GetIgnoreCollision_Injected(IntPtr collider1, IntPtr collider2);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Internal_RaycastAll_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Ray ray,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction,
    out BlittableArrayWrapper ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Query_CapsuleCastAll_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 p0,
    [In] ref Vector3 p1,
    float radius,
    [In] ref Vector3 direction,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction,
    out BlittableArrayWrapper ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Query_SphereCastAll_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 origin,
    float radius,
    [In] ref Vector3 direction,
    float maxDistance,
    int mask,
    QueryTriggerInteraction queryTriggerInteraction,
    out BlittableArrayWrapper ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern Collider[] OverlapCapsule_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 point0,
    [In] ref Vector3 point1,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern Collider[] OverlapSphere_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 position,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Simulate_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    float step,
    SimulationStage stages,
    SimulationOption options);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void InterpolateBodies_Internal_Injected([In] ref PhysicsScene physicsScene);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void ResetInterpolationPoses_Internal_Injected([In] ref PhysicsScene physicsScene);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool Query_ComputePenetration_Injected(
    IntPtr colliderA,
    [In] ref Vector3 positionA,
    [In] ref Quaternion rotationA,
    IntPtr colliderB,
    [In] ref Vector3 positionB,
    [In] ref Quaternion rotationB,
    ref Vector3 direction,
    ref float distance);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Query_ClosestPoint_Injected(
    IntPtr collider,
    [In] ref Vector3 position,
    [In] ref Quaternion rotation,
    [In] ref Vector3 point,
    out Vector3 ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void get_clothGravity_Injected(out Vector3 ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void set_clothGravity_Injected([In] ref Vector3 value);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool CheckSphere_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 position,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool CheckCapsule_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 start,
    [In] ref Vector3 end,
    float radius,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern bool CheckBox_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 center,
    [In] ref Vector3 halfExtents,
    [In] ref Quaternion orientation,
    int layermask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern Collider[] OverlapBox_Internal_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 center,
    [In] ref Vector3 halfExtents,
    [In] ref Quaternion orientation,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void Internal_BoxCastAll_Injected(
    [In] ref PhysicsScene physicsScene,
    [In] ref Vector3 center,
    [In] ref Vector3 halfExtents,
    [In] ref Vector3 direction,
    [In] ref Quaternion orientation,
    float maxDistance,
    int layerMask,
    QueryTriggerInteraction queryTriggerInteraction,
    out BlittableArrayWrapper ret);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr GetColliderByInstanceID_Injected(int instanceID);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern IntPtr GetBodyByInstanceID_Injected(int instanceID);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void SendOnCollisionEnter_Injected(IntPtr component, Collision collision);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void SendOnCollisionStay_Injected(IntPtr component, Collision collision);

  [MethodImpl(MethodImplOptions.InternalCall)]
  private static extern void SendOnCollisionExit_Injected(IntPtr component, Collision collision);

  /// <summary>
  ///   <para></para>
  /// </summary>
  /// <param name="scene">The physics scene that the contacts belong to.</param>
  /// <param name="headerArray">A contact buffer where all the contact data of the previous simulation step is stored.</param>
  public delegate void ContactEventDelegate(
    PhysicsScene scene,
    NativeArray<ContactPairHeader>.ReadOnly headerArray);
}
