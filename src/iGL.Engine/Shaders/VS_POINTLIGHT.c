

precision mediump float;
//precision mediump int;

// Attributes
attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_uv;

// Shader variables
vec4 normal;
vec4 vertexPositionInEye;

// Structs
struct Light {	
	vec4 position; 
	vec4 ambient;
	vec4 diffuse;
	vec4 specular;	
};

struct Material {
	vec4 ambient;
	vec4 diffuse;
	vec4 specular;
	float shininess;
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform vec4 u_eyepos;

uniform Light u_light;
uniform Material u_material;
uniform bool u_hasTexture;

uniform vec4 u_globalAmbientColor;

varying vec4 v_ambientColor;
varying vec3 v_normal;
varying vec3 v_eyepos;
varying vec3 v_lightVector;
varying vec2 v_uv;

void calcLightning();

void main() 
{
	gl_Position =  u_modelViewProjectionMatrix * vec4(a_position.x, a_position.y, a_position.z, 1);
	v_normal =  vec3(u_transposeAdjointModelViewMatrix * vec4(a_normal.x, a_normal.y, a_normal.z, 1));	
	v_uv = a_uv;

	calcLightning();	
}

void calcLightning()
{	
	vec4 lightDir = u_light.position - u_modelViewMatrix*vec4(a_position.x, a_position.y, a_position.z, 1);
	
	v_lightVector = vec3(lightDir);

	v_ambientColor = u_material.ambient * u_globalAmbientColor + u_material.ambient*u_light.ambient;

	v_eyepos = vec3(u_eyepos.x, u_eyepos.y, u_eyepos.z);
}
