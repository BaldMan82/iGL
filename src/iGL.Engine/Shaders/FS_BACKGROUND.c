

struct Light {	
	lowp vec4 position; 
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;	
};

struct Material {
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;
	lowp float shininess;
};

// Uniforms
uniform Light u_light;
uniform Material u_material;
uniform mediump vec4 u_globalAmbientColor;
uniform mediump vec2 u_textureScale;

// Varyings
varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying lowp vec2 v_uv;

uniform sampler2D s_texture;

void main() 
{	
	lowp vec4 textureColor = texture2D(s_texture, vec2(v_uv.x*u_textureScale.x, v_uv.y*u_textureScale.y));
	if (textureColor.a < 0.1) discard;

	gl_FragColor = v_ambientColor*textureColor + v_diffuseColor*textureColor;
}
