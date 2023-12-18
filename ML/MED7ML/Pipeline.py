from sklearn.preprocessing import MinMaxScaler
import pandas as pd
from sklearn.model_selection import KFold
from sklearn.metrics import accuracy_score
from sklearn.ensemble import GradientBoostingClassifier


# Read csv file into a dataframe
def read_csv_to_dataframe(file_path):
    try:
        df = pd.read_csv(file_path)
        return df
    except FileNotFoundError:
        print(f"Error: File not found at path {file_path}")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None


# Get the data set, remove unwanted columns and scale certain columns.
df = read_csv_to_dataframe('Data/all_data_rdy1.csv')
df.drop('Index', axis=1, inplace=True)          # Index should not be used for the model
df.drop('Success', axis=1, inplace=True)        # Removing 'Success' as it only has a 0.000015 feature importance
scaler = MinMaxScaler(feature_range=(-1, 1))           # Normalizing the values to a range from -1 to 1
norm_coll = ['Player Hurt', 'Player Moving', 'Player Attacking']
df[norm_coll] = scaler.fit_transform(df[norm_coll])

# Separating the features from the target (Type)
X = df.drop('Type', axis=1)  # Features
y = df['Type']  # Target variable

# Using K-folds cross validation, num_folds being the amount of splits of the data set
num_folds = 7
kf = KFold(n_splits=num_folds, shuffle=True, random_state=109)
accuracy_scores = []

# Using the GBC model, with hyperparameters tested using a grid search to find the most accurate.
model1 = GradientBoostingClassifier(n_estimators=50, learning_rate=0.1)  # You can customize parameters as needed


# Perform k-fold cross-validation and GBC
for train_index, test_index in kf.split(X):
    X_train, X_test = X.iloc[train_index], X.iloc[test_index]
    y_train, y_test = y.iloc[train_index], y.iloc[test_index]

    # Train the model
    model1.fit(X_train, y_train)

    # Make predictions on the test set
    y_pred = model1.predict(X_test)

    # Evaluate the model and store the accuracy score
    accuracy = accuracy_score(y_test, y_pred)
    accuracy_scores.append(accuracy)

# Display the mean performance metric across all folds
print(f'Accuracy per fold: {accuracy_scores}')
print(f'Mean Accuracy across {num_folds} folds: {sum(accuracy_scores) / len(accuracy_scores)}')


# Create a DataFrame to display feature importances
feature_importance = model1.feature_importances_
feature_importance_df = pd.DataFrame({
    'Feature': X.columns,
    'Importance': feature_importance
})

# Sort the DataFrame by importance in descending order
feature_importance_df = feature_importance_df.sort_values(by='Importance', ascending=False)

# Display the feature importance DataFrame
print(feature_importance_df)
