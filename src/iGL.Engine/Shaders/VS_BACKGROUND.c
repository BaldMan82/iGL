
// Attributes
attribute mediump vec3 a_position;
attribute mediump vec3 a_normal;
attribute mediump vec2 a_uv;


// Structs
struct Light {	
	highp vec4 position; 
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;	
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform mediump vec2 u_centerPoint;
uniform mediump vec2 u_minBounds;
uniform mediump vec2 u_maxBounds;

//uniform lowp vec4 u_eyePos;

uniform Light u_light;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_color;
varying highp vec2 v_uv;

void calcLightning(mediump vec4 position, mediump vec4 normal);

void main() 
{
	highp vec4 position =  vec4(a_position.x, a_position.y, a_position.z, 1);		
	
	gl_Position = u_modelViewProjectionMatrix *position;

	highp float lx = abs(u_maxBounds.x - u_minBounds.x);
	highp float ly = abs(u_maxBounds.y - u_minBounds.y);

	highp vec4 transformedPosition = u_modelViewMatrix*position;
	calcLightning(transformedPosition, vec4(a_normal.x, a_normal.y, a_normal.z, 1));
	
	v_uv = vec2((transformedPosition.x - u_minBounds.x) / lx , 1.0 - (transformedPosition.y - u_minBounds.y) / ly);
}

void calcLightning(highp vec4 position, highp vec4 normal)
{	
	highp vec4 lightDir = u_light.position - position;
	highp vec4 transformedNormal = u_transposeAdjointModelViewMatrix*normal;
		
	lightDir = normalize(lightDir);
	transformedNormal = normalize(transformedNormal);

	highp float lambertTerm = dot(transformedNormal, lightDir);
		
	v_color = u_light.diffuse * clamp(lambertTerm*2, 0.0, 1.0);	
	v_color.a = 1;

}
