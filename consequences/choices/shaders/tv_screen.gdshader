shader_type spatial;
render_mode unshaded, cull_back;

void fragment() {
	vec2 uv = UV;
	float t = TIME;

	// Slow vertical roll — v-sync slipping on a dying TV
	uv.y = fract(uv.y + sin(t * 0.08) * 0.015 + t * 0.011);

	// Slight horizontal wobble along scan rows
	uv.x += sin(uv.y * 28.0 + t * 2.3) * 0.005;

	// Four overlapping low-frequency sine waves.
	// Different phases and speeds create vague blob shapes that
	// suggest figures, furniture, a room — without showing anything clearly.
	float s = 0.0;
	s += sin(uv.x * 3.5 + t * 0.9)  * sin(uv.y * 2.8 - t * 0.6);
	s += sin(uv.x * 2.0 - t * 0.4 + 1.5) * sin(uv.y * 3.2 + t * 0.8) * 0.7;
	s += sin((uv.x + uv.y) * 2.5   + t * 0.5) * 0.5;
	s += sin(uv.x * 1.2 + uv.y * 1.8 - t * 0.3) * 0.4;
	s = s * 0.28 + 0.5; // normalise to ~0..1

	// Warm phosphor palette — white, cream, occasional cold blue-white.
	// Hue drifts slowly so it never quite settles on a colour.
	float ph = t * 0.22;
	vec3 col;
	col.r = sin(s * 4.8 + ph)         * 0.28 + 0.66;
	col.g = sin(s * 4.8 + ph + 0.9)   * 0.23 + 0.63;
	col.b = sin(s * 4.8 + ph + 2.1)   * 0.30 + 0.58;
	col = clamp(col, 0.0, 1.0);

	// CRT scanlines (very subtle — just a darkening every other line)
	float scan = sin(UV.y * 220.0) * 0.035;
	col -= scan;

	// One horizontal interference bar rolling slowly downward
	float bar = fract(t * 0.35);
	col += smoothstep(0.018, 0.0, abs(fract(uv.y) - bar)) * 0.4;

	// Low-res analog noise grain
	vec2 gc = floor(uv * 48.0 + vec2(floor(t * 20.0), 0.0));
	float grain = fract(sin(dot(gc, vec2(127.1, 311.7))) * 43758.5453);
	col = mix(col, vec3(grain), 0.065);

	// CRT vignette — screen edges are noticeably darker
	vec2 vig = UV * 2.0 - 1.0;
	float vignette = 1.0 - dot(vig * vig, vec2(0.30));
	col *= clamp(vignette, 0.08, 1.0);

	// Phosphor brightness boost
	col = clamp(col * 1.35, 0.0, 1.0);

	ALBEDO = col;
}
