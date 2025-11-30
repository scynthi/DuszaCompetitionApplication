shader_type canvas_item;

uniform sampler2D screenText : hint_screen_texture;
uniform int color_depth : hint_range(2, 256) = 16;

void fragment() {
    vec4 color = texture(screenText, SCREEN_UV);
    
    float levels = float(color_depth);
    color.rgb = floor(color.rgb * levels) / levels;
    
    COLOR = color;
}