

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
uniform Light u_light;
uniform Material u_material;
uniform lowp vec4 u_globalAmbientColor;
uniform bool u_hasTexture;

// Varyings
varying lowp vec4 v_ambientColor;
varying lowp vec3 v_normal;
varying lowp vec3 v_eyepos;
varying lowp vec3 v_lightVector;
varying lowp vec2 v_uv;

uniform sampler2D s_texture;

void calcLightning(out mediump vec4 color, mediump vec4 textureColor);

void main() 
{	
	mediump vec4 color;
	mediump vec4 textureColor = texture2D(s_texture, v_uv);
	calcLightning(color, textureColor);
	
	gl_FragColor = color; 
}

void calcLightning(out mediump vec4 color, mediump vec4 textureColor)
{
	color = v_ambientColor;
	if (u_hasTexture) color *= textureColor;

	mediump vec3 N = normalize(v_normal);
	mediump vec3 L = normalize(v_lightVector);
	
	lowp float lambertTerm = dot(N,L);
	
	if(lambertTerm > 0.0)
	{
		lowp vec4 addColor = u_light.diffuse * 
		               u_material.diffuse * 
					   lambertTerm;	

		if (u_hasTexture) addColor *= textureColor;
		
		color += addColor;

		/*vec3 E = normalize(v_eyepos);
		vec3 R = reflect(-L, N);
		float specular = pow( max(dot(R, E), 0.0), 
		                 u_material.shininess );
		color += u_light.specular * 
		               u_material.specular * 
					   specular;	*/
	}
}
