using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using weka.core.converters;
using weka.core;

namespace DecisionLayer
{
    public class DecisionMaking
    {

        const int percentSplit = 66;
        public static double classifyTest(weka.classifiers.Classifier cl, string file)
        {
            file = file.Replace("\\", "/");
            double rate = 0;
            try
            {
                string[] csv = file.Split('.');
                weka.core.Instances insts;
                if (csv[1] == "csv")
                {
                    CSVLoader loader = new CSVLoader();
                    loader.setOptions(new String[] { "-H" });
                    loader.setSource(new java.io.File(file));

                    insts = loader.getDataSet();
                }
                else
                {
                    insts = new weka.core.Instances(new java.io.FileReader(file));

                }
                insts.setClassIndex(insts.numAttributes() - 1);

                Console.WriteLine("Performing " + percentSplit + "% split evaluation.");

                weka.filters.Filter normalized = new weka.filters.unsupervised.attribute.Normalize();
                normalized.setInputFormat(insts);
                insts = weka.filters.Filter.useFilter(insts, normalized);

                //randomize the order of the instances in the dataset.
                weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
                myRandom.setInputFormat(insts);
                insts = weka.filters.Filter.useFilter(insts, myRandom);

                int trainSize = insts.numInstances() * percentSplit / 100;
                int testSize = insts.numInstances() - trainSize;
                weka.core.Instances train = new weka.core.Instances(insts, 0, trainSize);

                cl.buildClassifier(train);


                int numCorrect = 0;
                for (int i = trainSize; i < insts.numInstances(); i++)
                {
                    weka.core.Instance currentInst = insts.instance(i);
                    double predictedClass = cl.classifyInstance(currentInst);
                    if (predictedClass == insts.instance(i).classValue())
                        numCorrect++;
                }

                rate = (double)((double)numCorrect / (double)testSize * 100.0);

            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace();
            }

            return rate;
        }


        public static Dictionary<string, Object> Decide(string filePath)
        {
            double[] rate = new double[15];
            String[] rateNames = {"J48", "RandomForest", "RandomTree",
             "REPTree", "LMT", "MultilayerPerceptron", "SMO", "Logistic","LinearRegression","NaiveBayes",
            "IBk(1)","IBk(3)","IBk(5)","IBk(7)","IBk(9)"};
            rate[0] = classifyTest(new weka.classifiers.trees.J48(), filePath);
            rate[1] = classifyTest(new weka.classifiers.trees.RandomForest(), filePath);
            rate[2] = classifyTest(new weka.classifiers.trees.RandomTree(), filePath);
            rate[3] = classifyTest(new weka.classifiers.trees.REPTree(), filePath);
            rate[4] = classifyTest(new weka.classifiers.trees.LMT(), filePath);
            rate[5] = classifyTest(new weka.classifiers.functions.MultilayerPerceptron(), filePath);
            rate[6] = classifyTest(new weka.classifiers.functions.SMO(), filePath);
            rate[7] = classifyTest(new weka.classifiers.functions.Logistic(), filePath);
            rate[8] = classifyTest(new weka.classifiers.functions.LinearRegression(), filePath);
            rate[9] = classifyTest(new weka.classifiers.bayes.NaiveBayes(), filePath);
            rate[10] = classifyTest(new weka.classifiers.lazy.IBk(1), filePath); // k = 1 için
            rate[11] = classifyTest(new weka.classifiers.lazy.IBk(3), filePath); // k = 3 için
            rate[12] = classifyTest(new weka.classifiers.lazy.IBk(5), filePath); // k = 5 için
            rate[13] = classifyTest(new weka.classifiers.lazy.IBk(7), filePath); // k = 7 için
            rate[14] = classifyTest(new weka.classifiers.lazy.IBk(9), filePath); // k = 9 için

            return new Dictionary<string, Object>() { { "values", rate }, { "names", rateNames } };
        }
    }
}
