﻿using Terraria.Audio;
using Terraria.ModLoader;

namespace TerrariaOverhaul.Core.PhysicalMaterials
{
	public abstract class PhysicalMaterial : ModType
	{
		public virtual SoundStyle? HitSound => null;

		protected sealed override void Register()
		{
			PhysicalMaterialSystem.AddPhysicalMaterial(this);
		}
	}
}
