from setuptools import setup

setup(name='pyccflex',
      version='0.1',
      description='Cybersecurity Pattern Analysis System',
      url='https://github.com/miroslawstaron/cybersecurity_pattern_analysis_system',
      author='Miroslaw Staron',
      author_email='miroslaw.staron@gu.se',
      license='Apache-2.0',
      install_requires=[
          'torch',
          'numpy',
          'pandas',
          'scikit-learn',
          'transformers',
          ],
      zip_safe=False)
