#version 420

uniform mat4 u_modelViewProjMatrix;
uniform mat4 u_modelWorldMatrix;
uniform mat4 u_worldViewMatrix;
uniform mat4 u_normalMatrix;

// attributes of our vertex
in vec3 v_position;
in vec4 v_color;	// vertex color
in vec3 v_normal;

out vec3 f_normal;
out vec4 f_color;
out vec3 f_position;

void main()
{
    vec3 normal = normalize(v_normal);
	
	// gl_Position is a special variable of OpenGL that must be set
    gl_Position = u_modelViewProjMatrix * vec4(v_position, 1);

	// fix normal after non-uniform scaling
	f_normal = (u_normalMatrix * vec4(normal, 0)).xyz;	
	f_color = v_color;
	f_position = vec3(u_modelWorldMatrix * vec4(v_position, 1));
}

//f_normal = (u_worldViewMatrix * u_modelWorldMatrix * vec4(v_normal, 0)).xyz;	// pass normal to frag shader
// w = 1 for point, w = 0 for vector