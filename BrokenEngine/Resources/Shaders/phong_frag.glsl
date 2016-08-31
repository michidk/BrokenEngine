#version 420

uniform mat4 u_worldViewMatrix;

uniform vec4 u_albedo;
uniform vec3 u_lightDirection;
uniform vec4 u_lightColor = vec4(1,1,1,1);
uniform vec4 u_ambientColor = vec4(0,0,0,1);

in vec3 f_normal;	// interpolated normal
in vec4 f_color;	// interpolated vertex color
in vec3 f_position;	// interpolated world position

out vec4 fragColor; // first out variable is automatically written to the screen

const float gamma = 2.2;
const bool blinn = true;
const float specularStrength = 0.5f;

void main()
{
	// WRONG
	vec3 camPos = vec3(u_worldViewMatrix[3][0], u_worldViewMatrix[3][1], u_worldViewMatrix[3][2]);
	
	// diffuse component
	vec3 normal = normalize(f_normal);
	float diff = dot(normal, u_lightDirection);
	vec4 diffuse = max(diff, 0) * u_lightColor;
	
	vec3 viewDir = normalize(camPos - f_position);
	vec3 reflectDir = reflect(-u_lightDirection, normal);
	float spec = dot(viewDir, reflectDir);
	float highlight = pow(max(spec, 0), 32);
	vec4 specular = specularStrength * highlight * u_lightColor;

	//vec4 result = (u_ambientColor + diffuse + specular) * u_albedo;
	vec4 result = (u_ambientColor + diffuse) * u_albedo;
	
	result.a = 1;
	fragColor = result;
	//fragColor = result * vec4(camPos,1);	// to test camPos
	
	// apply gamma correction
    fragColor.rgb = pow(fragColor.rgb, vec3(1.0/gamma));
}