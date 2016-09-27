//# Name Blinn-Phong Lightning
//# Author Michael Lohr
//# Description A blinn-phong lightning implementation

//# Type VERTEX
#version 420

// transformation matrices
uniform mat4 u_modelViewProjMatrix;
uniform mat4 u_modelWorldMatrix;
uniform mat4 u_normalMatrix;

// vertex attributes
in vec3 v_position;
in vec3 v_normal;
in vec4 v_color;

// vertex output
out vec3 f_position;
out vec3 f_normal;
out vec4 f_color;


void main()
{
	// always normalizing these normals
    vec3 normal = normalize(v_normal);

	// position as vector4 ready for matrice multiplication
	vec4 pos4 = vec4(v_position, 1);

	// gl_Position is a special variable of OpenGL that must be set
	// convert position in clip space for the rasterizer
    gl_Position = u_modelViewProjMatrix * pos4;
	
	// convert position & normals in world space, to better calculate lightning
	f_position = vec3(u_modelWorldMatrix * pos4);
	f_normal = vec3(u_normalMatrix * vec4(normal, 0));	
	f_color = v_color;
}

//# Type FRAGMENT
#version 420

uniform vec3 u_cameraPosition;

// material properties
uniform vec4 u_albedo = vec4(1, 0, 1, 1);

uniform vec3 u_lightDirection;
uniform vec4 u_lightColor = vec4(1, 1, 1, 1);
uniform float u_lightIntensity = 1.0;

uniform float u_specularIntensity = 1.0;
uniform float u_specularShininess = 4.0;	// specular exponent: how big the highlight should be

uniform vec4 u_ambientColor = vec4(0, 0, 0, 1);
uniform float u_ambientIntensity = 1.0;

uniform bool blinn = true;					// use blinn highlights?

// shader input
in vec3 f_position;	// interpolated world position
in vec3 f_normal;	// interpolated normal
in vec4 f_color;	// interpolated vertex color

// shader output
out vec4 f_fragColor; // first out variable is automatically written to the screen


vec4 diffuse(vec3 normal) {
	float lightAmount = dot(normal, u_lightDirection);
	return max(lightAmount, 0) * u_lightColor;
}

vec4 specular(vec3 normal) {
	vec3 viewDir = normalize(u_cameraPosition - f_position);
	
	// fork for blinn highlights
	float specularAmount;
	// fixes phong cutoff issue with point lightning and is cheaper to calculate because no reflection is needed
	if (blinn)
	{
		vec3 halfwayDir = normalize(u_lightDirection + viewDir);
		specularAmount = dot(normal, halfwayDir);
	}
	else
	{
		vec3 reflectDir = reflect(-u_lightDirection, normal);
		specularAmount = dot(viewDir, reflectDir);
	}

	float shininessFactor = blinn ? 3.0 : 1.0;	// shininess should be 2 to 4 times larger when using blinn highlights
	float highlight = pow(max(specularAmount, 0.0), u_specularShininess * shininessFactor);

	return highlight * u_specularIntensity * u_lightColor;
}

vec4 ambient() {
	return u_ambientColor * u_ambientIntensity;
}

void main()
{
	// variables
	vec3 normal = normalize(f_normal);
	
	// components
	vec4 diffuse = diffuse(normal);
	vec4 specular = specular(normal);
	vec4 ambient = ambient();

	// final composition
	vec4 result = (diffuse + specular + ambient) * u_albedo;
	
	// output
	result.a = 1;			// ignore alpha value
	f_fragColor = result;
}