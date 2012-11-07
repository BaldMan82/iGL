
// Attributes
attribute highp vec3 a_position;
attribute highp vec3 a_normal;
attribute highp vec2 a_uv;


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

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
//uniform lowp vec4 u_eyePos;

uniform Light u_light;
uniform Material u_material;

uniform highp float u_shortFloatFactor;

uniform mediump vec4 u_globalAmbientColor;

void main() 
{
	highp vec4 position =  vec4(a_position.x / u_shortFloatFactor, a_position.y / u_shortFloatFactor, a_position.z / u_shortFloatFactor, 1);
	
	gl_Position = u_modelViewProjectionMatrix *position;
}

