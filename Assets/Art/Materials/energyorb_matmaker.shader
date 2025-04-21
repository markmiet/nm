Shader "Unlit/NewUnlitShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			Blend One One
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float dot2(float2 x) {
	return dot(x, x);
}
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float param_rndi(float minimum, float maximum, float seed) {
	return floor(param_rnd(minimum, maximum + 1.0, seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
static const float p_o2503479407957_cx = 0.000000000;
static const float p_o2503479407957_cy = 0.000000000;
static const float p_o2503479407957_scale_x = 0.600000000;
static const float p_o2503747843405_cc_0_x = 0.000000000;
static const float p_o2503747843405_cc_0_y = 0.000000000;
static const float p_o2503747843405_cc_0_ls = 0.000000000;
static const float p_o2503747843405_cc_0_rs = 0.328107000;
static const float p_o2503747843405_cc_1_x = 1.000000000;
static const float p_o2503747843405_cc_1_y = 1.000000000;
static const float p_o2503747843405_cc_1_ls = 0.263882000;
static const float p_o2503747843405_cc_1_rs = -0.000000000;
static const float p_o2503747843405_cr_0_x = 0.000000000;
static const float p_o2503747843405_cr_0_y = 0.000000000;
static const float p_o2503747843405_cr_0_ls = 0.000000000;
static const float p_o2503747843405_cr_0_rs = 1.000000000;
static const float p_o2503747843405_cr_1_x = 1.000000000;
static const float p_o2503747843405_cr_1_y = 1.000000000;
static const float p_o2503747843405_cr_1_ls = 1.000000000;
static const float p_o2503747843405_cr_1_rs = 0.000000000;
static const float p_o2503747843405_cg_0_x = 0.000000000;
static const float p_o2503747843405_cg_0_y = 0.000000000;
static const float p_o2503747843405_cg_0_ls = 0.000000000;
static const float p_o2503747843405_cg_0_rs = 1.000000000;
static const float p_o2503747843405_cg_1_x = 1.000000000;
static const float p_o2503747843405_cg_1_y = 1.000000000;
static const float p_o2503747843405_cg_1_ls = 1.000000000;
static const float p_o2503747843405_cg_1_rs = 0.000000000;
static const float p_o2503747843405_cb_0_x = 0.000000000;
static const float p_o2503747843405_cb_0_y = 0.000000000;
static const float p_o2503747843405_cb_0_ls = 0.000000000;
static const float p_o2503747843405_cb_0_rs = 1.000000000;
static const float p_o2503747843405_cb_1_x = 1.000000000;
static const float p_o2503747843405_cb_1_y = 1.000000000;
static const float p_o2503747843405_cb_1_ls = 1.000000000;
static const float p_o2503747843405_cb_1_rs = 0.000000000;
static const float p_o2503462630740_value = 0.500000000;
static const float p_o2503462630740_width = 0.920000000;
static const float p_o2503512962386_amount1 = 5.000000000;
static const float p_o2503512962386_amount2 = 1.000000000;
static const float p_o2503512962386_amount3 = 1.000000000;
static const float p_o2503512962386_amount4 = 1.000000000;
static const float p_o2503512962386_amount5 = 1.000000000;
static const float p_o2503059977572_default_in1 = 0.000000000;
static const float p_o2503059977572_default_in2 = 16.000000000;
static const float p_o2504435709251_bevel = 1.000000000;
static const float p_o2504435709251_base = 1.000000000;
static const float p_o2503043200358_r = 0.000000000;
static const float p_o2503043200358_ripples = 1.000000000;
static const float p_o2503026423141_r = 0.390000000;
static const float p_o2503026423141_cx = 0.000000000;
static const float p_o2503026423141_cy = 0.000000000;
static const float4 p_o2503076754787_color = tofloat4(0.275269002, 0.661791980, 1.000000000, 1.000000000);
static const float4 p_o2503093532002_color = tofloat4(0.137254998, 0.219607994, 0.349020004, 1.000000000);
static const float p_o2503110309217_default_in1 = 0.000000000;
static const float p_o2503110309217_default_in2 = 0.000000000;
static const float p_o2503529739601_repeat = 1.000000000;
static const float p_o2503529739601_gradient_pos[2] = float[]( 0.000000000, 0.364340991 );
static const float4 p_o2503529739601_gradient_col[2] = float4[]( tofloat4(0.234375000, 0.234375000, 0.234375000, 1.000000000), tofloat4(1.000000000, 1.000000000, 1.000000000, 1.000000000) );
static const float p_o2503160640862_default_in1 = 0.000000000;
static const float p_o2503160640862_default_in2 = 0.000000000;
static const float p_o2503194195292_default_in1 = 0.000000000;
static const float p_o2503194195292_default_in2 = 1.000000000;
static const float p_o2503177418077_value = 0.231900000;
static const float p_o2503177418077_width = 1.000000000;
static const float p_o2503177418077_contrast = 0.100000000;
static const float seed_o2503210972507 = 0.000000000;
static const float p_o2503210972507_sx = 1.000000000;
static const float p_o2503210972507_sy = 1.000000000;
static const float p_o2503210972507_rotate = 0.000000000;
static const float p_o2503210972507_scale = 0.000000000;
static const float seed_o2503143863647 = 0.849920392;
static const float p_o2503143863647_scale_x = 12.000000000;
static const float p_o2503143863647_scale_y = 1.000000000;
static const float p_o2503143863647_folds = 0.000000000;
static const float p_o2503143863647_iterations = 3.000000000;
static const float p_o2503143863647_persistence = 0.500000000;
static const float p_o2503227749722_repeat = 1.000000000;
static const float p_o2503227749722_gradient_pos[2] = float[]( 0.000000000, 1.000000000 );
static const float4 p_o2503227749722_gradient_col[2] = float4[]( tofloat4(0.000000000, 0.000000000, 0.000000000, 1.000000000), tofloat4(1.000000000, 1.000000000, 1.000000000, 1.000000000) );
static const float p_o2503445853526_sides = 6.000000000;
static const float p_o2503445853526_radius = 2.000000000;
static const float p_o2503445853526_edge = 1.000000000;
static const float p_o2502942537064_sides = 6.000000000;
static const float p_o2502942537064_radius = 0.730000000;
static const float p_o2502942537064_edge = 1.330000000;
static const float seed_o2503127086432 = 0.000000000;
static const float p_o2503127086432_v1 = 0.000000000;
static const float p_o2503127086432_v2 = 0.100000000;
static const float p_o2503127086432_v3 = 0.200000000;
static const float p_o2503127086432_v4 = 0.300000000;
static const float p_o2503127086432_v5 = 0.400000000;
static const float p_o2502875428204_radius = 1.800000000;
static const float p_o2502875428204_repeat = 1.000000000;
static const float p_o2502892205419_translate_x = 0.000000000;
static const float p_o2502892205419_translate_y = -1.000000000;
static const float p_o2502892205419_rotate = 0.000000000;
static const float p_o2502892205419_scale_x = 1.000000000;
static const float p_o2503496185171_translate_x = 0.000000000;
static const float p_o2502925759849_value = 0.440000000;
static const float p_o2502925759849_width = 0.480000000;
static const float p_o2502925759849_contrast = 0.000000000;
static const float p_o2502959314279_amount = 0.105000000;
static const float p_o2502959314279_eps = 0.100000000;
static const float seed_o2502908982634 = 0.312437505;
static const float p_o2502908982634_scale_x = 17.000000000;
static const float p_o2502908982634_scale_y = 10.000000000;
static const float p_o2502908982634_folds = 0.000000000;
static const float p_o2502908982634_iterations = 2.000000000;
static const float p_o2502908982634_persistence = 0.500000000;
static const float p_o2502908982634_offset = 0.000000000;
// #globals: scale (o2503479407957)
float2 scale(float2 uv, float2 center, float2 scale) {
	uv -= center;
	uv /= scale;
	uv += center;
	return uv;
}
// #globals: tonality_4 (o2503747843405)
// #globals: blend
float3 blend_normal(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1 + (1.0-opacity)*c2;
}
float3 blend_dissolve(float2 uv, float3 c1, float3 c2, float opacity) {
	if (rand(uv) < opacity) {
		return c1;
	} else {
		return c2;
	}
}
float3 blend_multiply(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1*c2 + (1.0-opacity)*c2;
}
float3 blend_screen(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*(1.0-(1.0-c1)*(1.0-c2)) + (1.0-opacity)*c2;
}
float blend_overlay_f(float c1, float c2) {
	return (c1 < 0.5) ? (2.0*c1*c2) : (1.0-2.0*(1.0-c1)*(1.0-c2));
}
float3 blend_overlay(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_overlay_f(c1.x, c2.x), blend_overlay_f(c1.y, c2.y), blend_overlay_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hard_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*0.5*(c1*c2+blend_overlay(uv, c1, c2, 1.0)) + (1.0-opacity)*c2;
}
float blend_soft_light_f(float c1, float c2) {
	return (c2 < 0.5) ? (2.0*c1*c2+c1*c1*(1.0-2.0*c2)) : 2.0*c1*(1.0-c2)+sqrt(c1)*(2.0*c2-1.0);
}
float3 blend_soft_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_soft_light_f(c1.x, c2.x), blend_soft_light_f(c1.y, c2.y), blend_soft_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_burn_f(float c1, float c2) {
	return (c1==0.0)?c1:max((1.0-((1.0-c2)/c1)),0.0);
}
float3 blend_burn(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_burn_f(c1.x, c2.x), blend_burn_f(c1.y, c2.y), blend_burn_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_dodge_f(float c1, float c2) {
	return (c1==1.0)?c1:min(c2/(1.0-c1),1.0);
}
float3 blend_dodge(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_dodge_f(c1.x, c2.x), blend_dodge_f(c1.y, c2.y), blend_dodge_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_lighten(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*max(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_darken(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*min(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_difference(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*clamp(c2-c1, tofloat3(0.0), tofloat3(1.0)) + (1.0-opacity)*c2;
}
float3 blend_additive(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + c1 * oppacity;
}
float3 blend_addsub(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + (c1 - .5) * 2.0 * oppacity;
}
// #globals: adjust_hsv
float3 rgb_to_hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv_to_rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
// #globals: blend2_3 (o2503512962386)
float blend_linear_light_f(float c1, float c2) {
	return (c1 + 2.0 * c2) - 1.0;
}
float3 blend_linear_light(float2 uv, float3 c1, float3 c2, float opacity) {
return opacity*tofloat3(blend_linear_light_f(c1.x, c2.x), blend_linear_light_f(c1.y, c2.y), blend_linear_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_vivid_light_f(float c1, float c2) {
	return (c1 < 0.5) ? 1.0 - (1.0 - c2) / (2.0 * c1) : c2 / (2.0 * (1.0 - c1));
}
float3 blend_vivid_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_vivid_light_f(c1.x, c2.x), blend_vivid_light_f(c1.y, c2.y), blend_vivid_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_pin_light_f( float c1, float c2) {
	return (2.0 * c1 - 1.0 > c2) ? 2.0 * c1 - 1.0 : ((c1 < 0.5 * c2) ? 2.0 * c1 : c2);
}
float3 blend_pin_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_pin_light_f(c1.x, c2.x), blend_pin_light_f(c1.y, c2.y), blend_pin_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_hard_lerp_f(float c1, float c2) {
	return floor(c1 + c2);
}
float3 blend_hard_lerp(float2 uv, float3 c1, float3 c2, float opacity) {
		return opacity*tofloat3(blend_hard_lerp_f(c1.x, c2.x), blend_hard_lerp_f(c1.y, c2.y), blend_hard_lerp_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_exclusion_f(float c1, float c2) {
	return c1 + c2 - 2.0 * c1 * c2;
}
float3 blend_exclusion(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_exclusion_f(c1.x, c2.x), blend_exclusion_f(c1.y, c2.y), blend_exclusion_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hue(float2 uv, float3 c1, float3 c2, float opacity) {
	float3 outcol = c2;
	float3 hsv, hsv2, tmp;
	hsv2 = rgb_to_hsv(c1);
	if (hsv2.y != 0.0) {
		hsv = rgb_to_hsv(outcol);
		hsv.x = hsv2.x;
		tmp = hsv_to_rgb(hsv);
		outcol = lerp(outcol, tmp, opacity);
	}
	return outcol;
}
float3 blend_saturation(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 outcol = c2;
	float3 hsv, hsv2;
	hsv = rgb_to_hsv(outcol);
	if (hsv.y != 0.0) {
		hsv2 = rgb_to_hsv(c1);
		hsv.y = facm * hsv.y + opacity * hsv2.y;
		outcol = hsv_to_rgb(hsv);
	}
	return outcol;
}
float3 blend_color(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 outcol = c2;
	float3 hsv, hsv2, tmp;
	hsv2 = rgb_to_hsv(c1);
	if (hsv2.y != 0.0) {
		hsv = rgb_to_hsv(outcol);
		hsv.x = hsv2.x;
		hsv.y = hsv2.y;
		tmp = hsv_to_rgb(hsv);
		outcol = lerp(outcol, tmp, opacity);
	}
	return outcol;
}
float3 blend_value(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 hsv, hsv2;
	hsv = rgb_to_hsv(c2);
	hsv2 = rgb_to_hsv(c1);
	hsv.z = facm * hsv.z + opacity * hsv2.z;
	return hsv_to_rgb(hsv);
}
// #globals: math_5 (o2503059977572)
float pingpong(float a, float b)
{
  return (b != 0.0) ? abs(frac((a - b) / (b * 2.0)) * b * 2.0 - b) : 0.0;
}
// #globals: sdannularshape (o2503043200358)
float sdRipples(float d, float w, int r) {
	for (int i = 0; i < r; ++i) {
		d = abs(d)-w;
	}
	return d;
}
// #globals: fbm2_3 (o2503143863647)
float value_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float p00 = rand(mod(o, size));
	float p01 = rand(mod(o + tofloat2(0.0, 1.0), size));
	float p10 = rand(mod(o + tofloat2(1.0, 0.0), size));
	float p11 = rand(mod(o + tofloat2(1.0, 1.0), size));
	p00 = sin(p00 * 6.28318530718 + offset * 6.28318530718) / 2.0 + 0.5;
	p01 = sin(p01 * 6.28318530718 + offset * 6.28318530718) / 2.0 + 0.5;
	p10 = sin(p10 * 6.28318530718 + offset * 6.28318530718) / 2.0 + 0.5;
	p11 = sin(p11 * 6.28318530718 + offset * 6.28318530718) / 2.0 + 0.5;
	float2 t =  f * f * f * (f * (f * 6.0 - 15.0) + 10.0);
	return lerp(lerp(p00, p10, t.x), lerp(p01, p11, t.x), t.y);
}
float fbm_2d_value(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = value_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float perlin_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float a00 = rand(mod(o, size)) * 6.28318530718 + offset * 6.28318530718;
	float a01 = rand(mod(o + tofloat2(0.0, 1.0), size)) * 6.28318530718 + offset * 6.28318530718;
	float a10 = rand(mod(o + tofloat2(1.0, 0.0), size)) * 6.28318530718 + offset * 6.28318530718;
	float a11 = rand(mod(o + tofloat2(1.0, 1.0), size)) * 6.28318530718 + offset * 6.28318530718;
	float2 v00 = tofloat2(cos(a00), sin(a00));
	float2 v01 = tofloat2(cos(a01), sin(a01));
	float2 v10 = tofloat2(cos(a10), sin(a10));
	float2 v11 = tofloat2(cos(a11), sin(a11));
	float p00 = dot(v00, f);
	float p01 = dot(v01, f - tofloat2(0.0, 1.0));
	float p10 = dot(v10, f - tofloat2(1.0, 0.0));
	float p11 = dot(v11, f - tofloat2(1.0, 1.0));
	float2 t =  f * f * f * (f * (f * 6.0 - 15.0) + 10.0);
	return 0.5 + lerp(lerp(p00, p10, t.x), lerp(p01, p11, t.x), t.y);
}
float fbm_2d_perlin(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = perlin_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float perlinabs_noise_2d(float2 coord, float2 size, float offset, float seed) {
	return abs(2.0*perlin_noise_2d(coord, size, offset, seed)-1.0);
}
float fbm_2d_perlinabs(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = perlinabs_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float fbm_2d_mod289(float x) {
	return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float fbm_2d_permute(float x) {
	return fbm_2d_mod289(((x * 34.0) + 1.0) * x);
}
float2 fbm_2d_rgrad2(float2 p, float rot, float seed) {
	float u = fbm_2d_permute(fbm_2d_permute(p.x) + p.y) * 0.0243902439 + rot; // Rotate by shift
	u = frac(u) * 6.28318530718; // 2*pi
	return tofloat2(cos(u), sin(u));
}
float simplex_noise_2d(float2 coord, float2 size, float offset, float seed) {
	coord *= 2.0; // needed for it to tile
	coord += rand2(tofloat2(seed, 1.0-seed)) + size;
	size *= 2.0; // needed for it to tile
	coord.y += 0.001;
	float2 uv = tofloat2(coord.x + coord.y*0.5, coord.y);
	float2 i0 = floor(uv);
	float2 f0 = frac(uv);
	float2 i1 = (f0.x > f0.y) ? tofloat2(1.0, 0.0) : tofloat2(0.0, 1.0);
	float2 p0 = tofloat2(i0.x - i0.y * 0.5, i0.y);
	float2 p1 = tofloat2(p0.x + i1.x - i1.y * 0.5, p0.y + i1.y);
	float2 p2 = tofloat2(p0.x + 0.5, p0.y + 1.0);
	i1 = i0 + i1;
	float2 i2 = i0 + tofloat2(1.0, 1.0);
	float2 d0 = coord - p0;
	float2 d1 = coord - p1;
	float2 d2 = coord - p2;
	float3 xw = mod(tofloat3(p0.x, p1.x, p2.x), size.x);
	float3 yw = mod(tofloat3(p0.y, p1.y, p2.y), size.y);
	float3 iuw = xw + 0.5 * yw;
	float3 ivw = yw;
	float2 g0 = fbm_2d_rgrad2(tofloat2(iuw.x, ivw.x), offset, seed);
	float2 g1 = fbm_2d_rgrad2(tofloat2(iuw.y, ivw.y), offset, seed);
	float2 g2 = fbm_2d_rgrad2(tofloat2(iuw.z, ivw.z), offset, seed);
	float3 w = tofloat3(dot(g0, d0), dot(g1, d1), dot(g2, d2));
	float3 t = 0.8 - tofloat3(dot(d0, d0), dot(d1, d1), dot(d2, d2));
	t = max(t, tofloat3(0.0));
	float3 t2 = t * t;
	float3 t4 = t2 * t2;
	float n = dot(t4, w);
	return 0.5 + 5.5 * n;
}
float fbm_2d_simplex(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = simplex_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node =  0.5 + 0.25 * sin(offset * 6.28318530718 + 6.28318530718 * node);
			float2 diff = neighbor + node - f;
			float dist = length(diff);
			min_dist = min(min_dist, dist);
		}
	}
	return min_dist;
}
float fbm_2d_cellular(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular2_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist1 = 2.0;
	float min_dist2 = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node = 0.5 + 0.25 * sin(offset * 6.28318530718 + 6.28318530718*node);
			float2 diff = neighbor + node - f;
			float dist = length(diff);
			if (min_dist1 > dist) {
				min_dist2 = min_dist1;
				min_dist1 = dist;
			} else if (min_dist2 > dist) {
				min_dist2 = dist;
			}
		}
	}
	return min_dist2-min_dist1;
}
float fbm_2d_cellular2(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular2_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular3_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node = 0.5 + 0.25 * sin(offset * 6.28318530718 + 6.28318530718*node);
			float2 diff = neighbor + node - f;
			float dist = abs((diff).x) + abs((diff).y);
			min_dist = min(min_dist, dist);
		}
	}
	return min_dist;
}
float fbm_2d_cellular3(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular3_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular4_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist1 = 2.0;
	float min_dist2 = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node = 0.5 + 0.25 * sin(offset * 6.28318530718 + 6.28318530718*node);
			float2 diff = neighbor + node - f;
			float dist = abs((diff).x) + abs((diff).y);
			if (min_dist1 > dist) {
				min_dist2 = min_dist1;
				min_dist1 = dist;
			} else if (min_dist2 > dist) {
				min_dist2 = dist;
			}
		}
	}
	return min_dist2-min_dist1;
}
float fbm_2d_cellular4(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular4_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular5_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node = 0.5 + 0.5 * sin(offset * 6.28318530718 + 6.28318530718*node);
			float2 diff = neighbor + node - f;
			float dist = max(abs((diff).x), abs((diff).y));
			min_dist = min(min_dist, dist);
		}
	}
	return min_dist;
}
float fbm_2d_cellular5(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular5_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
float cellular6_noise_2d(float2 coord, float2 size, float offset, float seed) {
	float2 o = floor(coord)+rand2(tofloat2(seed, 1.0-seed))+size;
	float2 f = frac(coord);
	float min_dist1 = 2.0;
	float min_dist2 = 2.0;
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			float2 neighbor = tofloat2(float(x),float(y));
			float2 node = rand2(mod(o + tofloat2(x, y), size)) + tofloat2(x, y);
			node = 0.5 + 0.25 * sin(offset * 6.28318530718 + 6.28318530718*node);
			float2 diff = neighbor + node - f;
			float dist = max(abs((diff).x), abs((diff).y));
			if (min_dist1 > dist) {
				min_dist2 = min_dist1;
				min_dist1 = dist;
			} else if (min_dist2 > dist) {
				min_dist2 = dist;
			}
		}
	}
	return min_dist2-min_dist1;
}
float fbm_2d_cellular6(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = cellular6_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
// MIT License Inigo Quilez - https://www.shadertoy.com/view/Xd23Dh
float voronoise_noise_2d( float2 coord, float2 size, float offset, float seed) {
	float2 i = floor(coord) + rand2(tofloat2(seed, 1.0-seed)) + size;
	float2 f = frac(coord);
	
	float2 a = tofloat2(0.0);
	
	for( int y=-2; y<=2; y++ ) {
		for( int x=-2; x<=2; x++ ) {
			float2  g = tofloat2( float(x), float(y) );
			float3  o = rand3( mod(i + g, size) + tofloat2(seed) );
			o.xy += 0.25 * sin(offset * 6.28318530718 + 6.28318530718*o.xy);
			float2  d = g - f + o.xy;
			float w = pow( 1.0-smoothstep(0.0, 1.414, length(d)), 1.0 );
			a += tofloat2(o.z*w,w);
		}
	}
	
	return a.x/a.y;
}
float fbm_2d_voronoise(float2 coord, float2 size, int folds, int octaves, float persistence, float offset, float seed) {
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 1.0;
	for (int i = 0; i < octaves; i++) {
		float noise = voronoise_noise_2d(coord*size, size, offset, seed);
		for (int f = 0; f < folds; ++f) {
			noise = abs(2.0*noise-1.0);
		}
		value += noise * scale;
		normalize_factor += scale;
		size *= 2.0;
		scale *= persistence;
	}
	return value / normalize_factor;
}
// #globals: custom_uv (o2503210972507)
float2 get_from_tileset(float count, float seed, float2 uv) {
	return clamp((uv+floor(rand2(tofloat2(seed))*count))/count, tofloat2(0.0), tofloat2(1.0));
}
float2 custom_uv_transform(float2 uv, float2 cst_scale, float rnd_rotate, float rnd_scale, float2 seed) {
	seed = rand2(seed);
	uv -= tofloat2(0.5);
	float angle = (seed.x * 2.0 - 1.0) * rnd_rotate;
	float ca = cos(angle);
	float sa = sin(angle);
	uv = tofloat2(ca*uv.x+sa*uv.y, -sa*uv.x+ca*uv.y);
	uv *= (seed.y-0.5)*2.0*rnd_scale+1.0;
	uv /= cst_scale;
	uv += tofloat2(0.5);
	return uv;
}
// #globals: shape_3 (o2503445853526)
float shape_circle(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float distance = length(uv);
	return clamp((1.0-distance/size)/edge, 0.0, 1.0);
}
float shape_polygon(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = hlsl_atan(uv.x, uv.y)+3.14159265359;
	float slice = 6.28318530718/sides;
	return clamp((1.0-(cos(floor(0.5+angle/slice)*slice-angle)*length(uv))/size)/edge, 0.0, 1.0);
}
float shape_star(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = hlsl_atan(uv.x, uv.y);
	float slice = 6.28318530718/sides;
	return clamp((1.0-(cos(floor(angle*sides/6.28318530718-0.5+2.0*step(frac(angle*sides/6.28318530718), 0.5))*slice-angle)*length(uv))/size)/edge, 0.0, 1.0);
}
float shape_curved_star(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = 2.0*(hlsl_atan(uv.x, uv.y)+3.14159265359);
	float slice = 6.28318530718/sides;
	return clamp((1.0-cos(floor(0.5+0.5*angle/slice)*2.0*slice-angle)*length(uv)/size)/edge, 0.0, 1.0);
}
float shape_rays(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = 0.5*max(edge, 1.0e-8)*size;
	float slice = 6.28318530718/sides;
	float angle = mod(hlsl_atan(uv.x, uv.y)+3.14159265359, slice)/slice;
	return clamp(min((size-angle)/edge, angle/edge), 0.0, 1.0);
}
// #globals: transform2 (o2502892205419)
float2 transform2_clamp(float2 uv) {
	return clamp(uv, tofloat2(0.0), tofloat2(1.0));
}
float2 transform2(float2 uv, float2 translate, float rotate, float2 scale) {
 	float2 rv;
	uv -= translate;
	uv -= tofloat2(0.5);
	rv.x = cos(rotate)*uv.x + sin(rotate)*uv.y;
	rv.y = -sin(rotate)*uv.x + cos(rotate)*uv.y;
	rv /= scale;
	rv += tofloat2(0.5);
	return rv;	
}
float o2503747843405_cc_curve_fct(float x) {
{
float dx = x - p_o2503747843405_cc_0_x;
float d = p_o2503747843405_cc_1_x - p_o2503747843405_cc_0_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o2503747843405_cc_0_y;
float yac = p_o2503747843405_cc_0_y + d*p_o2503747843405_cc_0_rs;
float ybc = p_o2503747843405_cc_1_y - d*p_o2503747843405_cc_1_ls;
float y2 = p_o2503747843405_cc_1_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
}
float o2503747843405_cr_curve_fct(float x) {
{
float dx = x - p_o2503747843405_cr_0_x;
float d = p_o2503747843405_cr_1_x - p_o2503747843405_cr_0_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o2503747843405_cr_0_y;
float yac = p_o2503747843405_cr_0_y + d*p_o2503747843405_cr_0_rs;
float ybc = p_o2503747843405_cr_1_y - d*p_o2503747843405_cr_1_ls;
float y2 = p_o2503747843405_cr_1_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
}
float o2503747843405_cg_curve_fct(float x) {
{
float dx = x - p_o2503747843405_cg_0_x;
float d = p_o2503747843405_cg_1_x - p_o2503747843405_cg_0_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o2503747843405_cg_0_y;
float yac = p_o2503747843405_cg_0_y + d*p_o2503747843405_cg_0_rs;
float ybc = p_o2503747843405_cg_1_y - d*p_o2503747843405_cg_1_ls;
float y2 = p_o2503747843405_cg_1_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
}
float o2503747843405_cb_curve_fct(float x) {
{
float dx = x - p_o2503747843405_cb_0_x;
float d = p_o2503747843405_cb_1_x - p_o2503747843405_cb_0_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o2503747843405_cb_0_y;
float yac = p_o2503747843405_cb_0_y + d*p_o2503747843405_cb_0_rs;
float ybc = p_o2503747843405_cb_1_y - d*p_o2503747843405_cb_1_ls;
float y2 = p_o2503747843405_cb_1_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
}
float4 o2503529739601_gradient_gradient_fct(float x) {
  if (x < p_o2503529739601_gradient_pos[0]) {
    return p_o2503529739601_gradient_col[0];
  } else if (x < p_o2503529739601_gradient_pos[1]) {
    return lerp(p_o2503529739601_gradient_col[0], p_o2503529739601_gradient_col[1], 0.5-0.5*cos(3.14159265359*(x-p_o2503529739601_gradient_pos[0])/(p_o2503529739601_gradient_pos[1]-p_o2503529739601_gradient_pos[0])));
  }
  return p_o2503529739601_gradient_col[1];
}
float4 o2503210972507_input_in(float2 uv, float _seed_variation_) {
// #output0: fbm2_3 (o2503143863647)
float o2503143863647_0_1_f = fbm_2d_perlin(uv, tofloat2(p_o2503143863647_scale_x, p_o2503143863647_scale_y), int(p_o2503143863647_folds), int(p_o2503143863647_iterations), p_o2503143863647_persistence, (_Time.y*.5), (seed_o2503143863647+frac(_seed_variation_)));
return tofloat4(tofloat3(o2503143863647_0_1_f), 1.0);
}
float4 o2503227749722_gradient_gradient_fct(float x) {
  if (x < p_o2503227749722_gradient_pos[0]) {
    return p_o2503227749722_gradient_col[0];
  } else if (x < p_o2503227749722_gradient_pos[1]) {
    return lerp(p_o2503227749722_gradient_col[0], p_o2503227749722_gradient_col[1], ((x-p_o2503227749722_gradient_pos[0])/(p_o2503227749722_gradient_pos[1]-p_o2503227749722_gradient_pos[0])));
  }
  return p_o2503227749722_gradient_col[1];
}
float o2502959314279_input_d(float2 uv, float _seed_variation_) {
// #output0: fbm2 (o2502908982634)
float o2502908982634_0_1_f = fbm_2d_voronoise(uv, tofloat2(p_o2502908982634_scale_x, p_o2502908982634_scale_y), int(p_o2502908982634_folds), int(p_o2502908982634_iterations), p_o2502908982634_persistence, p_o2502908982634_offset, (seed_o2502908982634+frac(_seed_variation_)));
return o2502908982634_0_1_f;
}
// #instance: warp (o2502959314279)
float2 o2502959314279_slope(float2 uv, float epsilon, float _seed_variation_) {
	return tofloat2(o2502959314279_input_d((frac(uv+tofloat2(epsilon, 0.0))), _seed_variation_)-o2502959314279_input_d((frac(uv-tofloat2(epsilon, 0.0))), _seed_variation_), o2502959314279_input_d((frac(uv+tofloat2(0.0, epsilon))), _seed_variation_)-o2502959314279_input_d((frac(uv-tofloat2(0.0, epsilon))), _seed_variation_));
}
float o2503127086432_input_in(float2 uv, float _seed_variation_) {
// #code: warp (o2502959314279)
float2 o2502959314279_0_slope = o2502959314279_slope(((transform2((tofloat2(frac(p_o2502875428204_repeat*hlsl_atan(uv.y-0.5, uv.x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length(uv-tofloat2(0.5))))), tofloat2(p_o2502892205419_translate_x*(2.0*1.0-1.0), p_o2502892205419_translate_y*(2.0*1.0-1.0)), p_o2502892205419_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o2502892205419_scale_x*(2.0*1.0-1.0), (1.5/(tofloat2(frac(p_o2502875428204_repeat*hlsl_atan(uv.y-0.5, uv.x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length(uv-tofloat2(0.5))))).y*2.0-1.0)*(2.0*1.0-1.0))))-tofloat2(p_o2503496185171_translate_x, (_Time.y*0.4))), p_o2502959314279_eps, _seed_variation_);
float2 o2502959314279_0_warp = o2502959314279_0_slope;
// #output0: fbm2 (o2502908982634)
float o2502908982634_0_1_f = fbm_2d_voronoise((((transform2((tofloat2(frac(p_o2502875428204_repeat*hlsl_atan(uv.y-0.5, uv.x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length(uv-tofloat2(0.5))))), tofloat2(p_o2502892205419_translate_x*(2.0*1.0-1.0), p_o2502892205419_translate_y*(2.0*1.0-1.0)), p_o2502892205419_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o2502892205419_scale_x*(2.0*1.0-1.0), (1.5/(tofloat2(frac(p_o2502875428204_repeat*hlsl_atan(uv.y-0.5, uv.x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length(uv-tofloat2(0.5))))).y*2.0-1.0)*(2.0*1.0-1.0))))-tofloat2(p_o2503496185171_translate_x, (_Time.y*0.4)))+p_o2502959314279_amount*o2502959314279_0_warp), tofloat2(p_o2502908982634_scale_x, p_o2502908982634_scale_y), int(p_o2502908982634_folds), int(p_o2502908982634_iterations), p_o2502908982634_persistence, p_o2502908982634_offset, (seed_o2502908982634+frac(_seed_variation_)));
// #output0: warp (o2502959314279)
float4 o2502959314279_0_1_rgba = tofloat4(tofloat3(o2502908982634_0_1_f), 1.0);
// #code: tones_range (o2502925759849)
float o2502925759849_0_step = clamp(((dot((o2502959314279_0_1_rgba).rgb, tofloat3(1.0))/3.0) - (p_o2502925759849_value))/max(0.0001, p_o2502925759849_width)+0.5, 0.0, 1.0);
float o2502925759849_0_false = clamp((min(o2502925759849_0_step, 1.0-o2502925759849_0_step) * 2.0) / (1.0 - p_o2502925759849_contrast), 0.0, 1.0);
float o2502925759849_0_true = 1.0-o2502925759849_0_false;
// #output0: tones_range (o2502925759849)
float o2502925759849_0_1_f = o2502925759849_0_false;
// #output0: translate (o2503496185171)
float4 o2503496185171_0_1_rgba = tofloat4(tofloat3(o2502925759849_0_1_f), 1.0);
// #output0: transform2 (o2502892205419)
float4 o2502892205419_0_1_rgba = o2503496185171_0_1_rgba;
// #output0: circle_map (o2502875428204)
float4 o2502875428204_0_1_rgba = o2502892205419_0_1_rgba;
return (dot((o2502875428204_0_1_rgba).rgb, tofloat3(1.0))/3.0);
}
		
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				float _seed_variation_ = 0.0;
				float2 uv = i.uv;

// #output0: sdcircle (o2503026423141)
float o2503026423141_0_1_sdf2d = length((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x))))-tofloat2(p_o2503026423141_cx+0.5, p_o2503026423141_cy+0.5))-p_o2503026423141_r;

// #output0: sdannularshape (o2503043200358)
float o2503043200358_0_1_sdf2d = sdRipples(o2503026423141_0_1_sdf2d, p_o2503043200358_r, int(p_o2503043200358_ripples));

// #output0: sdshow_3 (o2504435709251)
float o2504435709251_0_1_f = clamp(p_o2504435709251_base-o2503043200358_0_1_sdf2d/max(p_o2504435709251_bevel, 0.00001), 0.0, 1.0);

// #code: math_5 (o2503059977572)
float o2503059977572_0_clamp_false = pow(o2504435709251_0_1_f,p_o2503059977572_default_in2);
float o2503059977572_0_clamp_true = clamp(o2503059977572_0_clamp_false, 0.0, 1.0);
// #output0: math_5 (o2503059977572)
float o2503059977572_0_1_f = o2503059977572_0_clamp_false;

// #output0: uniform (o2503076754787)
float4 o2503076754787_0_1_rgba = p_o2503076754787_color;

// #output0: uniform_2 (o2503093532002)
float4 o2503093532002_0_1_rgba = p_o2503093532002_color;

// #output0: radial_gradient (o2503529739601)
float4 o2503529739601_0_1_rgba = o2503529739601_gradient_gradient_fct(frac(p_o2503529739601_repeat*1.41421356237*length(frac((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))))-tofloat2(0.5, 0.5))));

// #output0: circular_gradient (o2503227749722)
float4 o2503227749722_0_1_rgba = o2503227749722_gradient_gradient_fct(frac(p_o2503227749722_repeat*0.15915494309*hlsl_atan((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).y-0.5, (scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).x-0.5)));

// #code: custom_uv (o2503210972507)
float3 o2503210972507_0_map = ((o2503227749722_0_1_rgba).rgb);
float o2503210972507_0_rnd =  float((seed_o2503210972507+frac(_seed_variation_)))+o2503210972507_0_map.z;
// #output0: custom_uv (o2503210972507)
float4 o2503210972507_0_1_rgba = o2503210972507_input_in(get_from_tileset(1.0, o2503210972507_0_rnd, custom_uv_transform(o2503210972507_0_map.xy, tofloat2(p_o2503210972507_sx, p_o2503210972507_sy), p_o2503210972507_rotate*0.01745329251, p_o2503210972507_scale, tofloat2(o2503210972507_0_map.z, float((seed_o2503210972507+frac(_seed_variation_)))))), false ? o2503210972507_0_rnd : 0.0);

// #code: tones_range_2 (o2503177418077)
float o2503177418077_0_step = clamp(((dot((o2503210972507_0_1_rgba).rgb, tofloat3(1.0))/3.0) - (p_o2503177418077_value))/max(0.0001, p_o2503177418077_width)+0.5, 0.0, 1.0);
float o2503177418077_0_false = clamp((min(o2503177418077_0_step, 1.0-o2503177418077_0_step) * 2.0) / (1.0 - p_o2503177418077_contrast), 0.0, 1.0);
float o2503177418077_0_true = 1.0-o2503177418077_0_false;
// #output0: tones_range_2 (o2503177418077)
float o2503177418077_0_1_f = o2503177418077_0_false;

// #code: math_10 (o2503194195292)
float o2503194195292_0_clamp_false = o2503177418077_0_1_f+p_o2503194195292_default_in2;
float o2503194195292_0_clamp_true = clamp(o2503194195292_0_clamp_false, 0.0, 1.0);
// #output0: math_10 (o2503194195292)
float o2503194195292_0_1_f = o2503194195292_0_clamp_false;

// #output0: shape_3 (o2503445853526)
float o2503445853526_0_1_f = shape_circle((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), p_o2503445853526_sides, p_o2503445853526_radius*1.0, p_o2503445853526_edge*1.0);

// #code: math_3 (o2503160640862)
float o2503160640862_0_clamp_false = o2503445853526_0_1_f*o2503194195292_0_1_f;
float o2503160640862_0_clamp_true = clamp(o2503160640862_0_clamp_false, 0.0, 1.0);
// #output0: math_3 (o2503160640862)
float o2503160640862_0_1_f = o2503160640862_0_clamp_false;

// #code: math_6 (o2503110309217)
float o2503110309217_0_clamp_false = o2503160640862_0_1_f/(dot((o2503529739601_0_1_rgba).rgb, tofloat3(1.0))/3.0);
float o2503110309217_0_clamp_true = clamp(o2503110309217_0_clamp_false, 0.0, 1.0);
// #output0: math_6 (o2503110309217)
float o2503110309217_0_1_f = o2503110309217_0_clamp_false;

// #output0: shape_2 (o2502942537064)
float o2502942537064_0_1_f = shape_circle((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), p_o2502942537064_sides, p_o2502942537064_radius*1.0, p_o2502942537064_edge*1.0);

// #output1: variations_greyscale (o2503127086432)
float o2503127086432_1_1_f = o2503127086432_input_in((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), (seed_o2503127086432+frac(_seed_variation_))+p_o2503127086432_v2);

// #code: warp (o2502959314279)
float2 o2502959314279_0_slope = o2502959314279_slope(((transform2((tofloat2(frac(p_o2502875428204_repeat*hlsl_atan((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).y-0.5, (scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x))))-tofloat2(0.5))))), tofloat2(p_o2502892205419_translate_x*(2.0*1.0-1.0), p_o2502892205419_translate_y*(2.0*1.0-1.0)), p_o2502892205419_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o2502892205419_scale_x*(2.0*1.0-1.0), (1.5/(tofloat2(frac(p_o2502875428204_repeat*hlsl_atan((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).y-0.5, (scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x))))-tofloat2(0.5))))).y*2.0-1.0)*(2.0*1.0-1.0))))-tofloat2(p_o2503496185171_translate_x, (_Time.y*0.4))), p_o2502959314279_eps, _seed_variation_);
float2 o2502959314279_0_warp = o2502959314279_0_slope;
// #output0: fbm2 (o2502908982634)
float o2502908982634_0_1_f = fbm_2d_voronoise((((transform2((tofloat2(frac(p_o2502875428204_repeat*hlsl_atan((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).y-0.5, (scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x))))-tofloat2(0.5))))), tofloat2(p_o2502892205419_translate_x*(2.0*1.0-1.0), p_o2502892205419_translate_y*(2.0*1.0-1.0)), p_o2502892205419_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o2502892205419_scale_x*(2.0*1.0-1.0), (1.5/(tofloat2(frac(p_o2502875428204_repeat*hlsl_atan((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).y-0.5, (scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))).x-0.5)*0.15915494309), min(0.99999, 2.0/p_o2502875428204_radius*length((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x))))-tofloat2(0.5))))).y*2.0-1.0)*(2.0*1.0-1.0))))-tofloat2(p_o2503496185171_translate_x, (_Time.y*0.4)))+p_o2502959314279_amount*o2502959314279_0_warp), tofloat2(p_o2502908982634_scale_x, p_o2502908982634_scale_y), int(p_o2502908982634_folds), int(p_o2502908982634_iterations), p_o2502908982634_persistence, p_o2502908982634_offset, (seed_o2502908982634+frac(_seed_variation_)));

// #output0: warp (o2502959314279)
float4 o2502959314279_0_1_rgba = tofloat4(tofloat3(o2502908982634_0_1_f), 1.0);

// #code: tones_range (o2502925759849)
float o2502925759849_0_step = clamp(((dot((o2502959314279_0_1_rgba).rgb, tofloat3(1.0))/3.0) - (p_o2502925759849_value))/max(0.0001, p_o2502925759849_width)+0.5, 0.0, 1.0);
float o2502925759849_0_false = clamp((min(o2502925759849_0_step, 1.0-o2502925759849_0_step) * 2.0) / (1.0 - p_o2502925759849_contrast), 0.0, 1.0);
float o2502925759849_0_true = 1.0-o2502925759849_0_false;
// #output0: tones_range (o2502925759849)
float o2502925759849_0_1_f = o2502925759849_0_false;

// #output0: translate (o2503496185171)
float4 o2503496185171_0_1_rgba = tofloat4(tofloat3(o2502925759849_0_1_f), 1.0);

// #output0: transform2 (o2502892205419)
float4 o2502892205419_0_1_rgba = o2503496185171_0_1_rgba;

// #output0: circle_map (o2502875428204)
float4 o2502875428204_0_1_rgba = o2502892205419_0_1_rgba;

// #code: blend2_3 (o2503512962386)
float4 o2503512962386_0_b = o2502875428204_0_1_rgba;
float4 o2503512962386_0_l;
float o2503512962386_0_a;

o2503512962386_0_l = tofloat4(tofloat3(o2503127086432_1_1_f), 1.0);
o2503512962386_0_a = p_o2503512962386_amount1*1.0;
o2503512962386_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), o2503512962386_0_l.rgb, o2503512962386_0_b.rgb, o2503512962386_0_a*o2503512962386_0_l.a), min(1.0, o2503512962386_0_b.a+o2503512962386_0_a*o2503512962386_0_l.a));

o2503512962386_0_l = tofloat4(tofloat3(o2502942537064_0_1_f), 1.0);
o2503512962386_0_a = p_o2503512962386_amount2*1.0;
o2503512962386_0_b = tofloat4(blend_multiply((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), o2503512962386_0_l.rgb, o2503512962386_0_b.rgb, o2503512962386_0_a*o2503512962386_0_l.a), min(1.0, o2503512962386_0_b.a+o2503512962386_0_a*o2503512962386_0_l.a));

o2503512962386_0_l = tofloat4(tofloat3(o2503110309217_0_1_f), 1.0);
o2503512962386_0_a = p_o2503512962386_amount3*1.0;
o2503512962386_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), o2503512962386_0_l.rgb, o2503512962386_0_b.rgb, o2503512962386_0_a*o2503512962386_0_l.a), min(1.0, o2503512962386_0_b.a+o2503512962386_0_a*o2503512962386_0_l.a));

o2503512962386_0_l = o2503093532002_0_1_rgba;
o2503512962386_0_a = p_o2503512962386_amount4*1.0;
o2503512962386_0_b = tofloat4(blend_multiply((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), o2503512962386_0_l.rgb, o2503512962386_0_b.rgb, o2503512962386_0_a*o2503512962386_0_l.a), min(1.0, o2503512962386_0_b.a+o2503512962386_0_a*o2503512962386_0_l.a));

o2503512962386_0_l = o2503076754787_0_1_rgba;
o2503512962386_0_a = p_o2503512962386_amount5*o2503059977572_0_1_f;
o2503512962386_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o2503479407957_cx, 0.5+p_o2503479407957_cy), tofloat2(p_o2503479407957_scale_x, (p_o2503479407957_scale_x)))), o2503512962386_0_l.rgb, o2503512962386_0_b.rgb, o2503512962386_0_a*o2503512962386_0_l.a), min(1.0, o2503512962386_0_b.a+o2503512962386_0_a*o2503512962386_0_l.a));
// #output0: blend2_3 (o2503512962386)
float4 o2503512962386_0_1_rgba = o2503512962386_0_b;

// #code: tones_step (o2503462630740)
float3 o2503462630740_0_false = clamp((o2503512962386_0_1_rgba.rgb-tofloat3(p_o2503462630740_value))/max(0.0001, p_o2503462630740_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o2503462630740_0_true = tofloat3(1.0)-o2503462630740_0_false;
// #output0: tones_step (o2503462630740)
float4 o2503462630740_0_1_rgba = tofloat4(o2503462630740_0_false, o2503512962386_0_1_rgba.a);

// #output0: tonality_4 (o2503747843405)
float4 o2503747843405_0_1_rgba = tofloat4(tofloat3(o2503747843405_cc_curve_fct(o2503747843405_cr_curve_fct(o2503462630740_0_1_rgba.r)),o2503747843405_cc_curve_fct(o2503747843405_cg_curve_fct(o2503462630740_0_1_rgba.g)),o2503747843405_cc_curve_fct(o2503747843405_cb_curve_fct(o2503462630740_0_1_rgba.b))),o2503462630740_0_1_rgba.a);

// #output0: scale (o2503479407957)
float4 o2503479407957_0_1_rgba = o2503747843405_0_1_rgba;

				// sample the generated texture
				fixed4 col = o2503479407957_0_1_rgba;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}



