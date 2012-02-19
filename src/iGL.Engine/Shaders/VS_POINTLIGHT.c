

precision highp float;
precision highp int;

// Attributes
attribute vec4 a_position;
attribute vec4 a_color;
attribute vec3 a_normal;

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

uniform vec4 u_globalAmbientColor;

varying vec4 v_ambientColor;
varying vec3 v_normal;
varying vec3 v_eyepos;
varying vec3 v_lightVector;

void calcLightning();

void main() 
{
	gl_Position = u_modelViewProjectionMatrix * a_position;
	normal =  u_modelViewMatrix * vec4(a_normal.x, a_normal.y, a_normal.z, 1);
	v_normal = vec3(normal.x, normal.y, normal.z);

	calcLightning();	
}

void calcLightning()
{	
	vec4 lightDir = u_light.position - u_modelViewMatrix*a_position;
	
	v_lightVector = vec3(lightDir.x, lightDir.y, lightDir.z);

	v_ambientColor = u_material.ambient * u_globalAmbientColor + u_material.ambient*u_light.ambient;

	v_eyepos = vec3(u_eyepos.x, u_eyepos.y, u_eyepos.z);
}
