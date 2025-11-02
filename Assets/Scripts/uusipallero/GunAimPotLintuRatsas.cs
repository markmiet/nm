using UnityEngine;

/// <summary>
/// Handles smooth 2D gun aiming and positioning for a skeleton character.
/// Keeps the grip point locked to the hand, smoothly rotates toward the target,
/// and flips the sprite when aiming left.
/// </summary>
public class GunAimPotLintuRatsas : BaseController
{
    [Header("Viittaukset")]
    public Transform aseenkahva;      // aseen oma kiinnityskohta
    public Transform piipunkohta;     // aseen piipun pää (valinnainen)
    public Transform takatahtain;     // aseen takaosa (valinnainen)
    public Transform kadenPaikka;     // käden sijainti toisessa objektissa
    public Transform target;          // kohde, johon ase osoittaa

    [Header("Asetukset")]
    public bool flipY = false;        // jos haluat peilata aseen kun osoittaa vasemmalle
    public PotLintuRatKasiTahtain potLintuRatKasiTahtain;
    public bool pitaakoNahdajottavoiTahdata = true;

    public void Start()
    {
        target = PalautaAlus().transform;
    }

    public bool VoikoTahdata()
    {
        if (potLintuRatKasiTahtain==null || !pitaakoNahdajottavoiTahdata)
        {
            return true;
        }
        return potLintuRatKasiTahtain.CanEnemySeePlayer();
    }

    void LateUpdate()
    {
        if (aseenkahva == null || kadenPaikka == null) return;

        if (!VoikoTahdata())
        {
            return;
        }

        // 1️⃣ Siirrä ase siten, että aseenkahva on käden kohdalla
        Vector3 offset = transform.position - aseenkahva.position;
        transform.position = kadenPaikka.position + offset;

        // 2️⃣ Käännä ase kohti kohdetta
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Jos haluat että ase peilaa itsensä kun menee vasemmalle
            if (flipY)
            {
                Vector3 scale = transform.localScale;
                scale.y = (dir.x < 0) ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
                transform.localScale = scale;
            }
        }
    }

#if UNITY_EDITOR
    // Piirretään Editorissa suunta-viivat
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (piipunkohta != null)
            Gizmos.DrawSphere(piipunkohta.position, 0.05f);
        if (takatahtain != null)
            Gizmos.DrawSphere(takatahtain.position, 0.05f);
    }
#endif


}
