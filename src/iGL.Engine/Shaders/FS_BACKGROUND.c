

// Varyings
varying lowp vec4 v_color;
varying highp vec2 v_uv; 

uniform sampler2D s_texture;

void main() 
{	
	lowp vec4 textureColor = texture2D(s_texture, vec2(v_uv.x, v_uv.y));
	if (textureColor.a < 0.1) discard;

	gl_FragColor = v_color*textureColor;
	
}

