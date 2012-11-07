

// Structs
struct Light {	
	mediump vec4 position; 
	mediump vec4 ambient;
	mediump vec4 diffuse;
	mediump vec4 specular;	
};

struct Material {
	mediump vec4 ambient;
	mediump vec4 diffuse;
	mediump vec4 specular;
	mediump float shininess;	
};

// Uniforms
uniform Light u_light;
uniform Material u_material;
uniform mediump vec4 u_globalAmbientColor;

// Varyings
varying mediump vec4 v_ambientColor;
varying mediump vec4 v_diffuseColor;
varying mediump vec2 v_uv;

uniform sampler2D s_texture;

void main() 
{		
	gl_FragColor = u_material.ambient;
}
