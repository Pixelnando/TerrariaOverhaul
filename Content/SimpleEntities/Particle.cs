﻿using Microsoft.Xna.Framework;
using Terraria;
using TerrariaOverhaul.Common.Systems.Camera;
using TerrariaOverhaul.Common.Systems.Time;
using TerrariaOverhaul.Core.Systems.SimpleEntities;
using TerrariaOverhaul.Utilities;
using TerrariaOverhaul.Utilities.Extensions;

namespace TerrariaOverhaul.Content.SimpleEntities
{
	public abstract class Particle : SimpleEntity
	{
		public const float MaxParticleDistance = 3000f;
		public const float MaxParticleDistanceSqr = MaxParticleDistance * MaxParticleDistance;

		public float alpha = 1f;
		public float rotation;
		public Vector2 position;
		public Vector2 velocity;
		public Vector2 scale = Vector2.One;
		public Vector2 gravity = new Vector2(0f, 10f);
		public Color color;

		public virtual bool CollidesWithTiles => true;

		public override void Update()
		{
			if(Vector2.DistanceSquared(position, CameraSystem.ScreenCenter) >= MaxParticleDistanceSqr || position.HasNaNs()) {
				Destroy();

				return;
			}

			velocity += gravity * TimeSystem.LogicDeltaTime;

			if(CollidesWithTiles && Main.tile.TryGet((int)(position.X / 16), (int)(position.Y / 16), out var tile)) {
				if(tile.active() && Main.tileSolid[tile.type]) {
					Destroy();

					return;
				}

				if(tile.liquid > 0) {
					OnLiquidContact(tile, out bool destroy);

					if(destroy) {
						Destroy(true);
						return;
					}
				}
			}

			position += velocity * TimeSystem.LogicDeltaTime;
			scale = Vector2Utils.StepTowards(scale, Vector2.Zero, 0.1f * TimeSystem.LogicDeltaTime);

			if(scale.X < 0f || scale.Y < 0f || alpha <= 0f) {
				Destroy(true);
			}
		}

		protected virtual void OnLiquidContact(Tile tile, out bool destroy) => destroy = false;
	}
}