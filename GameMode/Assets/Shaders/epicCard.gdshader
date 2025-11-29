shader_type canvas_item;

uniform sampler2D scroll_texture : hint_default_white, filter_nearest, repeat_enable;
uniform vec2 scroll_speed = vec2(0.1, 0.0);
uniform float alpha : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    vec2 scrolled_uv = UV + TIME * scroll_speed;
    vec4 tex_color = texture(scroll_texture, scrolled_uv);

    COLOR = tex_color;
    COLOR.a *= alpha;
}