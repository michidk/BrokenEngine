#version 420

uniform mat4 u_modelViewProjMatrix;
uniform mat4 u_modelWorldMatrix;
uniform mat4 u_worldViewMatrix;

// attributes of our vertex
in vec3 v_position;
in vec4 v_color;	// vertex color
in vec3 v_normal;

out vec3 f_normal;
out vec3 f_rawNormal;
out vec4 f_color;

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = u_modelViewProjMatrix * vec4(v_position, 1);
	f_rawNormal = v_normal;
	f_normal = (u_worldViewMatrix * u_modelWorldMatrix * vec4(v_normal, 0)).xyz;	// pass normal to frag shader
	f_color = v_color;
}