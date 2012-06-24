
// Attributes
attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_uv;


// Structs
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

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewInverseMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform lowp vec4 u_eyepos;

uniform Light u_light;
uniform Material u_material;

uniform float u_hasTexture;
uniform float u_hasNormalTexture;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying lowp vec3 v_normal;
varying lowp vec3 v_eyepos;
varying lowp vec3 v_lightVector;
varying lowp vec2 v_uv;


void calcLightning();

void main() 
{
	gl_Position =  u_modelViewProjectionMatrix * vec4(a_position.x, a_position.y, a_position.z, 1);
	
	if (u_hasNormalTexture == 0)
	{
		v_normal =  vec3(u_transposeAdjointModelViewMatrix * vec4(a_normal.x, a_normal.y, a_normal.z, 1));	
	}

	v_uv = a_uv;

	calcLightning();	
	
	if (u_hasNormalTexture > 0)
	{
		v_lightVector = vec3(u_modelViewInverseMatrix * vec4(v_lightVector, 1));
	}
}

void calcLightning()
{	
	vec4 lightDir = u_light.position - u_modelViewMatrix*vec4(a_position.x, a_position.y, a_position.z, 1);
	
	v_lightVector = vec3(lightDir);

	v_ambientColor = u_material.ambient*u_globalAmbientColor  + u_material.ambient*u_light.ambient;
	
	v_eyepos = vec3(u_eyepos.x, u_eyepos.y, u_eyepos.z);
}
