shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;
uniform float color_depth : hint_range(2.0, 64.0) = 32.0;
uniform float dither_strength : hint_range(0.0, 2.0) = 0.8;
uniform float saturation : hint_range(0.0, 3.0) = 1.8;

const float BAYER[16] = float[16](
	 0.0 / 16.0,  8.0 / 16.0,  2.0 / 16.0, 10.0 / 16.0,
	12.0 / 16.0,  4.0 / 16.0, 14.0 / 16.0,  6.0 / 16.0,
	 3.0 / 16.0, 11.0 / 16.0,  1.0 / 16.0,  9.0 / 16.0,
	15.0 / 16.0,  7.0 / 16.0, 13.0 / 16.0,  5.0 / 16.0
);

float bayer(vec2 pos) {
	int x = int(mod(pos.x, 4.0));
	int y = int(mod(pos.y, 4.0));
	return BAYER[y * 4 + x] - 0.5;
}

vec3 sat_adjust(vec3 color, float s) {
	float luma = dot(color, vec3(0.299, 0.587, 0.114));
	return mix(vec3(luma), color, s);
}

void fragment() {
	vec2 uv = SCREEN_UV;
	vec3 col = texture(SCREEN_TEXTURE, uv).rgb;

	col = sat_adjust(col, saturation);

	float dither_offset = bayer(floor(FRAGCOORD.xy)) * dither_strength / color_depth;
	col += vec3(dither_offset);

	col = floor(col * color_depth + 0.5) / color_depth;
	col = clamp(col, 0.0, 1.0);

	COLOR = vec4(col, 1.0);
}
