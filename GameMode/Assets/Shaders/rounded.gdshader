shader_type canvas_item;

uniform float corner_radius = 10.0;

void fragment() {
    vec2 uv = UV;
    vec2 size = 1.0 / TEXTURE_PIXEL_SIZE;
    
    vec2 dist = min(uv * size, (vec2(1.0) - uv) * size);
    float min_dist = min(dist.x, dist.y);
    
    float alpha = smoothstep(corner_radius - 1.0, corner_radius, min_dist);
    
    COLOR = texture(TEXTURE, UV);
    COLOR.a *= alpha;
}