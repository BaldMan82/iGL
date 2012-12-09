
uniform mediump vec2 u_textureScale;
uniform int u_blackBorder;

struct Material {
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;
	lowp float shininess;
};

uniform Material u_material;

// Varyings
varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying highp vec2 v_uv;

uniform sampler2D s_texture;

void main() 
{	
	lowp vec4 textureColor = texture2D(s_texture, vec2(v_uv.x*u_textureScale.x, v_uv.y*u_textureScale.y));
	if (textureColor.a < 0.1) discard;

	if (u_blackBorder > 0 && textureColor.a < 1.0){
		gl_FragColor = vec4(0,0,0, textureColor.a);
	}
	else {
		lowp vec4 finalColor = v_ambientColor*textureColor + v_diffuseColor*textureColor;
		finalColor.a = u_material.ambient.a;
		
		if (finalColor.a == 0) discard;

		gl_FragColor = finalColor;
	}
	
}
