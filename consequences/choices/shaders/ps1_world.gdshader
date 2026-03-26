shader_type spatial;
render_mode diffuse_lambert, specular_disabled, cull_back;

uniform vec4 albedo : source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform float snap_resolution : hint_range(8.0, 512.0) = 64.0;
uniform float color_depth : hint_range(2.0, 64.0) = 32.0;

void vertex() {
	vec4 clip = PROJECTION_MATRIX * MODELVIEW_MATRIX * vec4(VERTEX, 1.0);
	clip.xy = round(clip.xy / clip.w * snap_resolution) / snap_resolution * clip.w;
	POSITION = clip;
}

void fragment() {
	vec3 col = albedo.rgb;
	col = floor(col * color_depth + 0.5) / color_depth;
	ALBEDO = col;
	ROUGHNESS = 1.0;
	SPECULAR = 0.0;
	METALLIC = 0.0;
}
