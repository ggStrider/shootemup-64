using Shoot;
using UnityEngine;

namespace Definitions.BulletModificators.Scripts
{
    [CreateAssetMenu(fileName = "Pierce Modificator", menuName = 
        StaticKeys.PROJECT_NAME + "/Bullet/Pierce Modificator")]
    public class PierceModificatorSO : BulletModificatorSO
    {
        public override void ApplyModificator(BulletBehaviour bullet)
        {
            var pierce = new BulletPierceModificator();
            pierce.ApplyModificator(bullet);
        }
    }
}