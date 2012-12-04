
// Attributes
attribute mediump vec3 a_position;
attribute mediump vec3 a_normal;
attribute mediump vec2 a_uv;

struct Material {
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;
	lowp float shininess;
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform mediump vec2 u_textureScale;
uniform int u_blackBorder;

uniform Material u_material;

uniform highp float u_shortFloatFactor;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying highp vec2 v_uv;

void main() 
{
	highp vec4 position =  vec4(a_position.x / u_shortFloatFactor, a_position.y / u_shortFloatFactor, a_position.z / u_shortFloatFactor, 1);

	v_uv = vec2(a_uv.x / u_shortFloatFactor, a_uv.y / u_shortFloatFactor);

	v_ambientColor = u_material.ambient*u_globalAmbientColor;	

	lowp vec3 v = a_normal;	
	
	gl_Position = u_modelViewProjectionMatrix *position;
}
