using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.D3DCompiler;
using Common.Util;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class EffectResource : ShaderResource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.Effect m_effect;
        private Dictionary<string, D3D11.EffectTechnique> m_effectTechniques;
        private Dictionary<string, D3D11.EffectPass> m_effectPasses;
        private Dictionary<string, D3D11.EffectVariable> m_effectVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compiling.</param>
        /// <param name="filePath">Path to shader file.</param>
        public EffectResource(string name, string shaderProfile, string filePath)
            : base(name, shaderProfile, filePath)
        {
            m_effectTechniques = new Dictionary<string, SlimDX.Direct3D11.EffectTechnique>();
            m_effectPasses = new Dictionary<string, SlimDX.Direct3D11.EffectPass>();
            m_effectVariables = new Dictionary<string, SlimDX.Direct3D11.EffectVariable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compiling.</param>
        /// <param name="resourceLink">The resource link.</param>
        public EffectResource(string name, string shaderProfile, AssemblyResourceLink resourceLink)
            : base(name, shaderProfile, resourceLink)
        {
            m_effectTechniques = new Dictionary<string, SlimDX.Direct3D11.EffectTechnique>();
            m_effectPasses = new Dictionary<string, SlimDX.Direct3D11.EffectPass>();
            m_effectVariables = new Dictionary<string, SlimDX.Direct3D11.EffectVariable>();
        }

        /// <summary>
        /// Gets the signatur of the given pass within the given technique.
        /// </summary>
        /// <param name="technique">The technique that contains the pass.</param>
        /// <param name="pass">The pass that holds the signature.</param>
        public ShaderSignature GetInputLayout(string technique, string pass)
        {
            string passKey = GetPassKey(technique, pass);
            if (!m_effectPasses.ContainsKey(passKey)) { throw new ArgumentException("Pass not found!", "technique, pass"); }

            return m_effectPasses[passKey].Description.Signature;
        }

        /// <summary>
        /// Applies the given pass and the given techique for the given device context.
        /// </summary>
        /// <param name="deviceContext">Target device context.</param>
        /// <param name="technique">The technique that contains the pass.</param>
        /// <param name="pass">The pass to apply.</param>
        public void ApplyPass(D3D11.DeviceContext deviceContext, string technique, string pass)
        {
            string passKey = GetPassKey(technique, pass);
            if (!m_effectPasses.ContainsKey(passKey)) { throw new ArgumentException("Pass not found!", "technique, pass"); }

            Result result = m_effectPasses[passKey].Apply(deviceContext);
        }

        /// <summary>
        /// Sets the given value to the given variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="matrix">the value.</param>
        public void SetVariableValue(string variableName, D3D11.ShaderResourceView value)
        {
            if (!m_effectVariables.ContainsKey(variableName)) { throw new ArgumentException("Variable not found!", "variableName"); }

            D3D11.EffectResourceVariable resourceVariable = m_effectVariables[variableName].AsResource();
            resourceVariable.SetResource(value);
        }

        /// <summary>
        /// Sets the given value to the given variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="matrix">the value.</param>
        public void SetVariableValue(string variableName, Matrix4 value)
        {
            if (!m_effectVariables.ContainsKey(variableName)) { throw new ArgumentException("Variable not found!", "variableName"); }

            D3D11.EffectMatrixVariable matrixVariable = m_effectVariables[variableName].AsMatrix();
            matrixVariable.SetMatrix(value.ToDirectXMatrix());
        }

        /// <summary>
        /// Sets the given value to the given variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="matrix">the value.</param>
        public void SetVariableValue(string variableName, Matrix value)
        {
            if (!m_effectVariables.ContainsKey(variableName)) { throw new ArgumentException("Variable not found!", "variableName"); }

            D3D11.EffectMatrixVariable matrixVariable = m_effectVariables[variableName].AsMatrix();
            matrixVariable.SetMatrix(value);
        }

        /// <summary>
        /// Sets the given value to the given variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="matrix">the value.</param>
        public void SetVariableValue(string variableName, float value)
        {
            if (!m_effectVariables.ContainsKey(variableName)) { throw new ArgumentException("Variable not found!", "variableName"); }

            D3D11.EffectScalarVariable scalarVariable = m_effectVariables[variableName].AsScalar();
            scalarVariable.Set(value);
        }

        /// <summary>
        /// Sets the given value to the given variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="matrix">the value.</param>
        public void SetVariableValue(string variableName, Vector3 value)
        {
            if (!m_effectVariables.ContainsKey(variableName)) { throw new ArgumentException("Variable not found!", "variableName"); }

            D3D11.EffectVectorVariable vectorVariable = m_effectVariables[variableName].AsVector();
            vectorVariable.Set(value.ToDirectXVector());
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        protected override void LoadShader(ShaderBytecode shaderBytecode)
        {
            if (m_effect == null)
            {
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;
                m_effect = new D3D11.Effect(device, shaderBytecode);

                AnalyseEffect();
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        protected override void UnloadShader()
        {
            m_effectVariables.Clear();
            m_effectPasses.Clear();
            m_effectTechniques.Clear();

            m_effect = GraphicsHelper.DisposeGraphicsObject(m_effect);
        }

        /// <summary>
        /// Analyses current effect.
        /// </summary>
        private void AnalyseEffect()
        {
            //Get all techniques and passes
            for (int loopTechnique = 0; loopTechnique < m_effect.Description.TechniqueCount; loopTechnique++)
            {
                D3D11.EffectTechnique effectTechnique = m_effect.GetTechniqueByIndex(loopTechnique);
                string techniqueName = effectTechnique.Description.Name;
                m_effectTechniques.Add(techniqueName, effectTechnique);
                for (int loopPass = 0; loopPass < effectTechnique.Description.PassCount; loopPass++)
                {
                    D3D11.EffectPass effectPass = effectTechnique.GetPassByIndex(loopPass);
                    string passName = effectPass.Description.Name;
                    m_effectPasses.Add(GetPassKey(techniqueName, passName), effectPass);
                }
            }

            //Get all variables
            for (int loopVariables = 0; loopVariables < m_effect.Description.GlobalVariableCount; loopVariables++)
            {
                D3D11.EffectVariable effectVariable = m_effect.GetVariableByIndex(loopVariables);
                string variableName = effectVariable.Description.Name;
                m_effectVariables[variableName] = effectVariable;
            }
        }

        /// <summary>
        /// Gets the key of a pass.
        /// </summary>
        /// <param name="technique">The technique wich contains the pass.</param>
        /// <param name="pass">The pass name.</param>
        private string GetPassKey(string technique, string pass)
        {
            return technique + "." + pass;
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_effect != null; }
        }

        /// <summary>
        /// Gets the loaded Effect object.
        /// </summary>
        public D3D11.Effect Effect
        {
            get { return m_effect; }
        }
    }
}
