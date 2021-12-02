namespace CryoFall.Automaton.Features
{
using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
  using AtomicTorch.CBND.CoreMod.Systems.Notifications;
  using AtomicTorch.CBND.CoreMod.Systems.Physics;
  using AtomicTorch.CBND.GameApi.Data.Physics;
  using AtomicTorch.CBND.GameApi.Data.World;
  using AtomicTorch.CBND.GameApi.Scripting;
  using AtomicTorch.GameEngine.Common.Primitives;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public abstract class ProtoFeatureNearbyObjects<T> : ProtoFeature<T>
    where T : class
  {
    private static List<string> ListLastNotification = new List<string>();

    /// <summary>
    /// Called by client component every tick.
    /// </summary>
    public override void Update(double deltaTime)
    {

    }

    public override void Stop()
    {
      base.Stop();

      ListLastNotification.Clear();
    }

    /// <summary>
    /// Called by client component on specific time interval.
    /// </summary>
    public override void Execute()
    {
      if (!(IsEnabled && CheckPrecondition()))
      {
        ListLastNotification.Clear();
        return;
      }

      FindTarget();
    }

    private void FindTarget()
    {
      var fromPos = CurrentCharacter.Position;
      using var objectsNearby = this.CurrentCharacter.PhysicsBody.PhysicsSpace
                                    .TestRectangle(new Vector2D(fromPos.X - 40.0, fromPos.Y - 40.0), new Vector2D(80.0, 80.0), null, false);

      var objectOfInterest = objectsNearby.AsList()
                             ?.Where(t => this.EnabledEntityList.Contains(t.PhysicsBody?.AssociatedWorldObject?.ProtoGameObject))
                             .ToList();

      if (objectOfInterest == null || objectOfInterest.Count == 0)
      {
        return;
      }

      foreach (var obj in objectOfInterest)
      {
        var testWorldObjectC = obj.PhysicsBody.AssociatedWorldObject as IStaticWorldObject;

        bool hideNotify = false;

        if (testWorldObjectC.ProtoGameObject is IProtoObjectVegetation protoObjectVegetation)
        {
          hideNotify = true;
          if (protoObjectVegetation.SharedGetGrowthProgress(testWorldObjectC) < 0.75)
            continue;
        }

        var targetPointC = CurrentCharacter.Position - testWorldObjectC.TilePosition.ToVector2D();

        this.Notify(testWorldObjectC, targetPointC, hideNotify);
      }
    }

    private void Notify(IWorldObject targetObject, Vector2D intersectionPoint, bool hideNotify)
    {
      if (object.Equals(targetObject, null))
        return;

      string notKey = targetObject.TilePosition.X.ToString() + ";" + targetObject.TilePosition.Y.ToString();

      if (ListLastNotification.Contains(notKey))
        return;

      var worldBoundsOffset = Api.Client.World.WorldBounds.Offset;
      ushort posX = (ushort)(targetObject.TilePosition.X - worldBoundsOffset.X);
      ushort posY = (ushort)(targetObject.TilePosition.Y - worldBoundsOffset.Y);

      string goTo =
       (intersectionPoint.Y >= 0 ? "DOWN" : "UP") +
       ", " +
       (intersectionPoint.X >= 0 ? "LEFT" : "RIGHT");

      string message = targetObject.ProtoGameObject.Name + " Mark(" + posX + ";" + posY + ";1) (" + goTo + ")";

      NotificationSystem.ClientShowNotification("Nearby Object", message, NotificationColor.Good, null, null, hideNotify, true, false);

      ListLastNotification.Add(notKey);
    }

    private Vector2D ShapeCenter(IPhysicsShape shape)
    {
      if (shape != null)
      {
        switch (shape.ShapeType)
        {
          case ShapeType.Rectangle:
            var shapeRectangle = (RectangleShape)shape;
            return shapeRectangle.Position + shapeRectangle.Size / 2d;
          case ShapeType.Point:
            var shapePoint = (PointShape)shape;
            return shapePoint.Point;
          case ShapeType.Circle:
            var shapeCircle = (CircleShape)shape;
            return shapeCircle.Center;
          case ShapeType.Line:
            break;
          case ShapeType.LineSegment:
            var lineSegmentShape = (LineSegmentShape)shape;
            return new Vector2D((lineSegmentShape.Point1.X + lineSegmentShape.Point2.X) / 2d,
                         (lineSegmentShape.Point1.Y + lineSegmentShape.Point2.Y) / 2d);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      return new Vector2D(0, 0);
    }
  }
}
