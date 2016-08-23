#version 420

uniform vec4 u_albedo;

in vec3 f_normal;	// interpolated normal
in vec4 f_color;	// interpolated vertex color
in vec3 f_rawNormal;

out vec4 fragColor; // first out variable is automatically written to the screen

void main()
{
	fragColor = vec4(f_rawNormal, 1);
	//fragColor = f_color;
	//float theta = dot(f_normal, l);
	//fragColor = u_albedo;
}